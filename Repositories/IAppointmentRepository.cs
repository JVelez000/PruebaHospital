using PruebaHospital.Models;
using System.Collections.Generic;

namespace PruebaHospital.Repositories
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();
        Appointment? GetById(int id);
        IEnumerable<Appointment> GetByPatientId(int patientId);
        IEnumerable<Appointment> GetByDoctorId(int doctorId);
        IEnumerable<Appointment> GetByStatus(AppointmentStatus status);
        
        bool HasDoctorAppointmentAtTime(int doctorId, DateTime date, TimeSpan time, int excludeAppointmentId = 0);
        bool HasPatientAppointmentAtTime(int patientId, DateTime date, TimeSpan time, int excludeAppointmentId = 0);
        bool IsAppointmentTimeAvailable(int doctorId, DateTime date, TimeSpan time, int excludeAppointmentId = 0);
        
        int GetAppointmentCountByStatus(AppointmentStatus status);
        IEnumerable<Appointment> GetTodayAppointments();
        IEnumerable<Appointment> GetUpcomingAppointments(int days = 7);
        
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
        void Save();
    }
}