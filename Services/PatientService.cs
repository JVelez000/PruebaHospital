using PruebaHospital.Models;
using PruebaHospital.Repositories;
using PruebaHospital.Validators;
using FluentValidation;

namespace PruebaHospital.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _repository;
        private readonly PatientValidator _validator;

        public PatientService(IPatientRepository repository, PatientValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _repository.GetAll();
        }

        public Patient? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Patient? GetByDocument(string document)
        {
            return _repository.GetByDocument(document);
        }

        public void Create(Patient patient)
        {
            _validator.ValidateAndThrow(patient);
            _repository.Add(patient);
            _repository.Save();
        }

        public void Update(Patient patient)
        {
            _validator.ValidateAndThrow(patient);
            _repository.Update(patient);
            _repository.Save();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }

        public bool IsDocumentUnique(string document)
        {
            return _repository.IsDocumentUnique(document);
        }
    }
}