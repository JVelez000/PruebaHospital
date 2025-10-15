using PruebaHospital.Models;
using System.Collections.Generic;

namespace PruebaHospital.Repositories
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetAll();
        Doctor? GetById(int id);
        Doctor? GetByDocument(string document);
        IEnumerable<Doctor> GetBySpeciality(string speciality);
        IEnumerable<string> GetSpecialities();
        bool IsDocumentUnique(string document);
        bool IsNameAndSpecialityUnique(string name, string speciality, int excludeId = 0);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(int id);
        void Save();
    }
}