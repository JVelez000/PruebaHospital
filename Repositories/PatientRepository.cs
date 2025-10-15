using Microsoft.EntityFrameworkCore;
using PruebaHospital.Models;
using PruebaHospital.Data;
using System.Collections.Generic;

namespace PruebaHospital.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patient? GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public Patient? GetByDocument(string document)
        {
            return _context.Patients.FirstOrDefault(p => p.Document == document);
        }

        public bool IsDocumentUnique(string document)
        {
            return !_context.Patients.Any(p => p.Document == document);
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
        }

        public void Create(Patient patient)
        {
            Add(patient);
            Save();
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        public void Delete(int id)
        {
            var patient = GetById(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}