using FluentValidation;
using PruebaHospital.Models;
using PruebaHospital.Repositories;

namespace PruebaHospital.Validators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentValidator(IAppointmentRepository repository)
        {
            _repository = repository;

            RuleFor(a => a.DoctorId)
                .GreaterThan(0).WithMessage("Debe seleccionar un médico");

            RuleFor(a => a.PatientId)
                .GreaterThan(0).WithMessage("Debe seleccionar un paciente");

            RuleFor(a => a.Date)
                .NotEmpty().WithMessage("La fecha es requerida")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha no puede ser en el pasado");

            RuleFor(a => a.Time)
                .NotEmpty().WithMessage("La hora es requerida");

            RuleFor(a => a.Reason)
                .MaximumLength(500).WithMessage("La razón no puede exceder 500 caracteres");
            
            RuleFor(a => a.Date)
                .Must(BeWeekday).WithMessage("Las citas solo pueden agendarse en días hábiles (lunes a viernes)");
            
            RuleFor(a => a.Time)
                .Must(BeWithinBusinessHours).WithMessage("El horario debe estar entre 8:00 AM y 6:00 PM");
        }

        private bool BeWeekday(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        private bool BeWithinBusinessHours(TimeSpan time)
        {
            return time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(18, 0, 0);
        }
    }
}