using PruebaHospital.Models;
using PruebaHospital.Repositories;
using PruebaHospital.Validators;
using PruebaHospital.Utils;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace PruebaHospital.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly AppointmentValidator _validator;
        private readonly IServiceProvider _serviceProvider;

        public AppointmentService(
            IAppointmentRepository repository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            AppointmentValidator validator, 
            IServiceProvider serviceProvider)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _validator = validator;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _repository.GetAll();
        }

        public Appointment? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Appointment> GetByPatientId(int patientId)
        {
            return _repository.GetByPatientId(patientId);
        }

        public IEnumerable<Appointment> GetByDoctorId(int doctorId)
        {
            return _repository.GetByDoctorId(doctorId);
        }

        public IEnumerable<Appointment> GetByStatus(AppointmentStatus status)
        {
            return _repository.GetByStatus(status);
        }

        public IEnumerable<Appointment> GetTodayAppointments()
        {
            return _repository.GetTodayAppointments();
        }

        public IEnumerable<Appointment> GetUpcomingAppointments(int days = 7)
        {
            return _repository.GetUpcomingAppointments(days);
        }

        public void Create(Appointment appointment)
        {
            try
            {
                _validator.ValidateAndThrow(appointment);
                
                if (_repository.HasDoctorAppointmentAtTime(appointment.DoctorId, appointment.Date, appointment.Time))
                {
                    throw new InvalidOperationException("El médico ya tiene una cita programada en ese horario.");
                }
                
                if (_repository.HasPatientAppointmentAtTime(appointment.PatientId, appointment.Date, appointment.Time))
                {
                    throw new InvalidOperationException("El paciente ya tiene una cita programada en ese horario.");
                }
                
                _repository.Add(appointment);
                _repository.Save();
                
                _ = Task.Run(async () => await TrySendConfirmationEmail(appointment));
                
            }
            catch (ValidationException ex)
            {
                throw new InvalidOperationException($"Error de validación: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}");
            }
        }

        public void Update(Appointment appointment)
        {
            try
            {
                _validator.ValidateAndThrow(appointment);
                
                if (_repository.HasDoctorAppointmentAtTime(appointment.DoctorId, appointment.Date, appointment.Time, appointment.Id))
                {
                    throw new InvalidOperationException("El médico ya tiene una cita programada en ese horario.");
                }
                
                if (_repository.HasPatientAppointmentAtTime(appointment.PatientId, appointment.Date, appointment.Time, appointment.Id))
                {
                    throw new InvalidOperationException("El paciente ya tiene una cita programada en ese horario.");
                }
                
                appointment.UpdatedAt = DateTime.Now;
                _repository.Update(appointment);
                _repository.Save();
                
                if (appointment.Status == AppointmentStatus.Confirmada || appointment.Status == AppointmentStatus.Pendiente)
                {
                    _ = Task.Run(async () => await TrySendConfirmationEmail(appointment));
                }
            }
            catch (ValidationException ex)
            {
                throw new InvalidOperationException($"Error de validación: {string.Join(", ", ex.Errors.Select(e => e.ErrorMessage))}");
            }
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
        
        public void MarkAsAttended(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null)
                throw new KeyNotFoundException("Cita no encontrada");

            appointment.Status = AppointmentStatus.Atendida;
            appointment.UpdatedAt = DateTime.Now;
            _repository.Update(appointment);
            _repository.Save();
        }

        public void Cancel(int id, string? reason = null)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null)
                throw new KeyNotFoundException("Cita no encontrada");

            appointment.Status = AppointmentStatus.Cancelada;
            appointment.Reason = reason ?? "Cancelada por el usuario";
            appointment.UpdatedAt = DateTime.Now;
            _repository.Update(appointment);
            _repository.Save();
        }

        public void Confirm(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null)
                throw new KeyNotFoundException("Cita no encontrada");

            appointment.Status = AppointmentStatus.Confirmada;
            appointment.UpdatedAt = DateTime.Now;
            _repository.Update(appointment);
            _repository.Save();
            
            _ = Task.Run(async () => await TrySendConfirmationEmail(appointment));
        }

        public void MarkAsNoShow(int id)
        {
            var appointment = _repository.GetById(id);
            if (appointment == null)
                throw new KeyNotFoundException("Cita no encontrada");

            appointment.Status = AppointmentStatus.NoAsistio;
            appointment.UpdatedAt = DateTime.Now;
            _repository.Update(appointment);
            _repository.Save();
        }
        
        public async Task<(bool Success, string Message)> SendReminderAsync(int appointmentId)
        {
            try
            {
                var appointment = _repository.GetById(appointmentId);
                if (appointment == null)
                    return (false, "Cita no encontrada");

                var patient = _patientRepository.GetById(appointment.PatientId);
                var doctor = _doctorRepository.GetById(appointment.DoctorId);

                if (patient == null || doctor == null)
                    return (false, "No se pudo obtener información del paciente o médico");

                using var scope = _serviceProvider.CreateScope();
                var emailHelper = scope.ServiceProvider.GetRequiredService<EmailHelper>();

                var (success, message) = await emailHelper.SendAppointmentReminderAsync(
                    patient.Email,
                    patient.FirstName,
                    doctor.Name,
                    appointment.Date,
                    appointment.Time.ToString(@"hh\:mm")
                );
                
                appointment.EmailSent = success;
                appointment.EmailSentAt = DateTime.Now;
                appointment.EmailStatus = success ? "Recordatorio enviado" : $"Recordatorio falló: {message}";
                _repository.Update(appointment);
                _repository.Save();

                return (success, message);
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar recordatorio: {ex.Message}");
            }
        }
        
        public async Task<(bool Success, string Message)> ResendConfirmationAsync(int appointmentId)
        {
            try
            {
                var appointment = _repository.GetById(appointmentId);
                if (appointment == null)
                    return (false, "Cita no encontrada");

                var (success, message) = await TrySendConfirmationEmail(appointment);
                return (success, success ? "Confirmación reenviada exitosamente" : message);
            }
            catch (Exception ex)
            {
                return (false, $"Error al reenviar confirmación: {ex.Message}");
            }
        }

        private async Task<(bool Success, string Message)> TrySendConfirmationEmail(Appointment appointment)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var scopedRepository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();
                var patientRepository = scope.ServiceProvider.GetRequiredService<IPatientRepository>();
                var doctorRepository = scope.ServiceProvider.GetRequiredService<IDoctorRepository>();
                var emailHelper = scope.ServiceProvider.GetRequiredService<EmailHelper>();
                
                var currentAppointment = scopedRepository.GetById(appointment.Id);
                var patient = patientRepository.GetById(currentAppointment.PatientId);
                var doctor = doctorRepository.GetById(currentAppointment.DoctorId);

                if (currentAppointment == null || patient == null || doctor == null)
                    return (false, "No se pudo obtener información completa de la cita");

                var (success, message) = await emailHelper.SendAppointmentConfirmationAsync(
                    patient.Email,
                    patient.FirstName,
                    doctor.Name,
                    doctor.Speciality,
                    currentAppointment.Date,
                    currentAppointment.Time.ToString(@"hh\:mm")
                );
                
                currentAppointment.EmailSent = success;
                currentAppointment.EmailSentAt = DateTime.Now;
                currentAppointment.EmailStatus = success ? "Enviado" : $"Falló: {message}";
                
                scopedRepository.Update(currentAppointment);
                scopedRepository.Save();

                return (success, message);
            }
            catch (Exception ex)
            {
                return (false, $"Error enviando email: {ex.Message}");
            }
        }
        
        public bool IsTimeSlotAvailable(int doctorId, DateTime date, TimeSpan time, int excludeAppointmentId = 0)
        {
            return _repository.IsAppointmentTimeAvailable(doctorId, date, time, excludeAppointmentId);
        }

        public Dictionary<AppointmentStatus, int> GetAppointmentStatistics()
        {
            return new Dictionary<AppointmentStatus, int>
            {
                { AppointmentStatus.Pendiente, _repository.GetAppointmentCountByStatus(AppointmentStatus.Pendiente) },
                { AppointmentStatus.Confirmada, _repository.GetAppointmentCountByStatus(AppointmentStatus.Confirmada) },
                { AppointmentStatus.Atendida, _repository.GetAppointmentCountByStatus(AppointmentStatus.Atendida) },
                { AppointmentStatus.Cancelada, _repository.GetAppointmentCountByStatus(AppointmentStatus.Cancelada) },
                { AppointmentStatus.NoAsistio, _repository.GetAppointmentCountByStatus(AppointmentStatus.NoAsistio) }
            };
        }

        public List<string> GetAvailableTimeSlots(int doctorId, DateTime date)
        {
            var availableSlots = new List<string>();
            var startTime = new TimeSpan(8, 0, 0);
            var endTime = new TimeSpan(18, 0, 0);
            
            for (var time = startTime; time <= endTime; time = time.Add(new TimeSpan(0, 30, 0)))
            {
                if (_repository.IsAppointmentTimeAvailable(doctorId, date, time))
                {
                    availableSlots.Add(time.ToString(@"hh\:mm"));
                }
            }
            
            return availableSlots;
        }
    }
}