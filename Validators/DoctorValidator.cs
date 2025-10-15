using FluentValidation;
using PruebaHospital.Models;
using PruebaHospital.Repositories;

namespace PruebaHospital.Validators
{
    public class DoctorValidator : AbstractValidator<Doctor>
    {
        private readonly IDoctorRepository _repository;

        public DoctorValidator(IDoctorRepository repository)
        {
            _repository = repository;

            RuleFor(d => d.Document)
                .NotEmpty().WithMessage("El documento es requerido")
                .Length(5, 20).WithMessage("El documento debe tener entre 5 y 20 caracteres")
                .Must(BeUniqueDocument).WithMessage("Ya existe un médico con este documento");

            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(d => d.Speciality)
                .NotEmpty().WithMessage("La especialidad es requerida")
                .MaximumLength(50).WithMessage("La especialidad no puede exceder 50 caracteres");

            RuleFor(d => d.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio")
                .EmailAddress().WithMessage("El formato del correo no es válido");

            RuleFor(d => d.Phone)
                .Matches(@"^\d{10}$").WithMessage("El teléfono debe tener exactamente 10 dígitos");
            
            RuleFor(d => d)
                .Must(BeUniqueNameAndSpeciality).WithMessage("Ya existe un médico con el mismo nombre y especialidad");
        }

        private bool BeUniqueDocument(string document)
        {
            return _repository.IsDocumentUnique(document);
        }

        private bool BeUniqueNameAndSpeciality(Doctor doctor)
        {
            return _repository.IsNameAndSpecialityUnique(doctor.Name, doctor.Speciality, doctor.Id);
        }
    }
}