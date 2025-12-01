using FluentValidation;
using Shop.Application.DTOs.Request.Shopping;

namespace Shop.Application.Validators.Shopping;

public class ProviderValidator : AbstractValidator<CreateProviderDto>
{
    public ProviderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del Proveedor no debe estar vacío")
            .Length(6, 50).WithMessage("El nombre del Proveedor debe tener mínimo 6 carácteres y máximo 50");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El Correo es Requerido")
            .EmailAddress().WithMessage("El formato del correo es inválido")
            .MaximumLength(100).WithMessage("El correo electrónico no puede exceder 100 caracteres");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El número de teléfono es requerido")
            .Must(DocumentValidator.CheckPhoneNumber)
            .WithMessage("El Formato de número de teléfono no es válido. Formatos válidos: 7812-3456, 2234-5678");
    }
}