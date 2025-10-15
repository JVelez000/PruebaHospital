using FluentValidation;
using PruebaHospital.Models;
using PruebaHospital.Repositories;

namespace PruebaHospital.Validators
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        private readonly IPatientRepository _repository;

        public PatientValidator(IPatientRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.Document)
                .NotEmpty().WithMessage("El documento de identidad es requerido")
                .Length(5, 20).WithMessage("El documento debe tener entre 5 y 20 caracteres")
                .Must(BeUniqueDocument).WithMessage("Ya existe un paciente con este documento");

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("El primer nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio")
                .EmailAddress().WithMessage("El formato del correo no es válido");

            RuleFor(p => p.Age)
                .GreaterThan(0).WithMessage("La edad debe ser mayor que cero")
                .LessThan(150).WithMessage("La edad debe ser menor a 150");

            RuleFor(p => p.Phone)
                .Matches(@"^\d{10}$").WithMessage("El teléfono debe tener exactamente 10 dígitos");
        }

        private bool BeUniqueDocument(string document)
        {
            return _repository.IsDocumentUnique(document);
        }
    }
}