using Microsoft.EntityFrameworkCore;
using PruebaHospital.Models;

namespace PruebaHospital.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasIndex(p => p.Document).IsUnique();
                entity.Property(p => p.Document).IsRequired().HasMaxLength(20);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Phone).IsRequired().HasMaxLength(10);
            });
            
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasIndex(d => d.Document).IsUnique();
                entity.Property(d => d.Document).IsRequired().HasMaxLength(20);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Speciality).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Email).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Phone).IsRequired().HasMaxLength(10);
            });
            
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(a => a.Reason).HasMaxLength(500);
                entity.Property(a => a.EmailStatus).HasMaxLength(100);
                
                entity.HasOne(a => a.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Doctor)
                    .WithMany(d => d.Appointments)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(a => a.Date);
                entity.HasIndex(a => a.Status);
                entity.HasIndex(a => new { a.DoctorId, a.Date, a.Time });
                entity.HasIndex(a => new { a.PatientId, a.Date, a.Time });
            });
        }
    }
}