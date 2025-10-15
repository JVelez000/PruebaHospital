using PruebaHospital.Models;
using PruebaHospital.Repositories;
using PruebaHospital.Validators;
using FluentValidation;

namespace PruebaHospital.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly DoctorValidator _validator;

        public DoctorService(IDoctorRepository repository, DoctorValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _repository.GetAll();
        }

        public Doctor? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Doctor? GetByDocument(string document)
        {
            return _repository.GetByDocument(document);
        }

        public IEnumerable<Doctor> GetBySpeciality(string speciality)
        {
            return _repository.GetBySpeciality(speciality);
        }

        public IEnumerable<string> GetSpecialities()
        {
            return _repository.GetSpecialities();
        }

        public void Create(Doctor doctor)
        {
            _validator.ValidateAndThrow(doctor);
            _repository.Add(doctor);
            _repository.Save();
        }

        public void Update(Doctor doctor)
        {
            _validator.ValidateAndThrow(doctor);
            _repository.Update(doctor);
            _repository.Save();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}