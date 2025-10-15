using PruebaHospital.Models;

namespace PruebaHospital.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Doctors.Any())
                return;

            var doctors = new List<Doctor>
            {
                new Doctor { Name = "Dr. Jhon Doe", Speciality = "Cardiology", Email = "drjhondoe@hospital.com", Phone = "3001234567" },
            };

            var patients = new List<Patient>
            {
                new Patient { FirstName = "Doe", LastName = "Jhon", Age = 45, Email = "doejhon@gmail.com", Phone = "3125551111" }
            };

            context.Doctors.AddRange(doctors);
            context.Patients.AddRange(patients);
            context.SaveChanges();
        }
    }
}