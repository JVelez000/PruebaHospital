using PruebaHospital.Models;

namespace PruebaHospital.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        bool IsDocumentUnique(string document);
        Patient? GetByDocument(string document);
    }
}