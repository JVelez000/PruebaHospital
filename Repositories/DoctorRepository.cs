using Microsoft.EntityFrameworkCore;
using PruebaHospital.Models;
using PruebaHospital.Data;
using System.Collections.Generic;

namespace PruebaHospital.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _context.Doctors.ToList();
        }

        public Doctor? GetById(int id)
        {
            return _context.Doctors.Find(id);
        }

        public Doctor? GetByDocument(string document)
        {
            return _context.Doctors.FirstOrDefault(d => d.Document == document);
        }

        public IEnumerable<Doctor> GetBySpeciality(string speciality)
        {
            return _context.Doctors
                .Where(d => d.Speciality.ToLower() == speciality.ToLower())
                .ToList();
        }

        public IEnumerable<string> GetSpecialities()
        {
            return _context.Doctors
                .Select(d => d.Speciality)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
        }

        public bool IsDocumentUnique(string document)
        {
            return !_context.Doctors.Any(d => d.Document == document);
        }

        public bool IsNameAndSpecialityUnique(string name, string speciality, int excludeId = 0)
        {
            return !_context.Doctors.Any(d => 
                d.Name.ToLower() == name.ToLower() && 
                d.Speciality.ToLower() == speciality.ToLower() && 
                d.Id != excludeId);
        }

        public void Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
        }

        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
        }

        public void Delete(int id)
        {
            var doctor = GetById(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}