using Microsoft.EntityFrameworkCore;
using PruebaHospital.Models;
using PruebaHospital.Data;
using System.Collections.Generic;

namespace PruebaHospital.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
        }

        public Appointment? GetById(int id)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Appointment> GetByPatientId(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
        }

        public IEnumerable<Appointment> GetByStatus(AppointmentStatus status)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
        }
        
        public bool HasDoctorAppointmentAtTime(int doctorId, DateTime date, TimeSpan time, int excludeAppointmentId = 0)
        {
            return _context.Appointments
                .Any(a => a.DoctorId == doctorId && 
                         a.Date == date && 
                         a.Time == time && 
                         a.Id != excludeAppointmentId &&
                         a.Status != AppointmentStatus.Cancelada);
        }

        public bool HasPatientAppointmentAtTime(int patientId, DateTime date, TimeSpan time, int excludeAppointmentId = 0)
        {
            return _context.Appointments
                .Any(a => a.PatientId == patientId && 
                         a.Date == date && 
                         a.Time == time && 
                         a.Id != excludeAppointmentId &&
                         a.Status != AppointmentStatus.Cancelada);
        }

        public bool IsAppointmentTimeAvailable(int doctorId, DateTime date, TimeSpan time, int excludeAppointmentId = 0)
        {
            return !HasDoctorAppointmentAtTime(doctorId, date, time, excludeAppointmentId);
        }
        
        public int GetAppointmentCountByStatus(AppointmentStatus status)
        {
            return _context.Appointments.Count(a => a.Status == status);
        }

        public IEnumerable<Appointment> GetTodayAppointments()
        {
            var today = DateTime.Today;
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Date == today && a.Status != AppointmentStatus.Cancelada)
                .OrderBy(a => a.Time)
                .ToList();
        }

        public IEnumerable<Appointment> GetUpcomingAppointments(int days = 7)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(days);
            
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Date >= startDate && a.Date <= endDate && a.Status != AppointmentStatus.Cancelada)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .ToList();
        }

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public void Delete(int id)
        {
            var appointment = GetById(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}