using FluentValidation;
using Shop.Application.DTOs.Request;

namespace Shop.Application.Validators;

public class CustomerValidator : AbstractValidator<RegisterCustomer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El Nombre de Usuario es Requerido")
            .Length(5, 50).WithMessage("El Nombre de Usuario debe tener entre 5 y 50 caracteres")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("El nombre de usuario solo puede contener letras, números y guiones bajos")
            .Must(NotContainSpaces).WithMessage("El nombre de usuario no puede contener espacios");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La Contraseña es requerida")
            .MinimumLength(8).WithMessage("La Contraseña debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("La Contraseña debe tener al menos: Una Mayúscula, Una Minúscula, Un número, y Un Carácter Especial");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de la Contraseña es requerida")
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");
        
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido")
            .Length(2, 100).WithMessage("El nombre completo debe tener entre 2 y 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La Fecha de Nacimiento es Requerida")
            .Must(CheckCustomerAge).WithMessage("Debes tener al menos 18 para Registrarte")
            .Must(ValidAge).WithMessage("La Fecha de Nacimiento no puede ser mayor a 100 Años");

        RuleFor(x => x.Dui)
            .NotEmpty().WithMessage("El DUI es requerido")
            .Matches(@"^\d{8}-\d{1}$")
            .WithMessage("\"El formato del DUI es inválido. Formato correcto: 12345678-9\"");
            //.Must(DocumentValidator.CheckDui).WithMessage("El DUI es Incorrecto");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La Dirección es requerida")
            .Length(20, 200).WithMessage("La Dirección debe tener entre 20 y 200 caracteres");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El número de teléfono es requerido")
            .Must(DocumentValidator.CheckPhoneNumber)
            .WithMessage("El Formato de número de teléfono no es válido. Formatos válidos: 7812-3456, 2234-5678");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El formato del correo electrónico es inválido")
            .MaximumLength(100).WithMessage("El correo electrónico no puede exceder 100 caracteres");
    }

    private bool NotContainSpaces(string username)
    {
        return !username.Contains(" ");
    }

    private bool CheckCustomerAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            age--;
            
        return age >= 18;
    }

    private bool ValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        return age <= 100;
    }
}