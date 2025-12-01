using FluentValidation;
using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;

namespace Shop.Application.Validators;

public class EmployeeValidator : AbstractValidator<RegisterEmployee>
{
    public EmployeeValidator()
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
        
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido")
            .Length(2, 100).WithMessage("El nombre completo debe tener entre 2 y 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("La Fecha de Nacimiento es requerida")
            .Must(CheckEmployeeAge).WithMessage("El Empleado debe tener entre 18 y 65 años");

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

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("La Fecha de Contratación es necesaria")
            .Must(ValidHireDate).WithMessage("La Fecha de Ingreso no puede ser futura")
            .Must(ReasonableHireDate).WithMessage("La fecha de ingreso no puede ser mayor a 50 años atrás");

        RuleFor(x => x.Salary)
            .NotEmpty().WithMessage("El Salario es requerido")
            .GreaterThan(0).WithMessage("El Salario debe ser mayor a 0")
            .LessThanOrEqualTo(50000).WithMessage("El salario no puede exceder $50,000")
            .Must(ValidSalaryFormat).WithMessage("El Salario debe tener máximo 2 decimales");

        RuleFor(x => x.Nit)
            .NotEmpty().WithMessage("El NIT es requerido")
            .Matches(@"^\d{4}-\d{6}-\d{3}-\d{1}$")
            .WithMessage("El formato del NIT es inválido. Formato correcto: 0614-241287-102-5");
            //.Must(DocumentValidator.CheckNit).WithMessage("El NIT es Invalido");
        
        RuleFor(x => x)
            .Must(x => CoherentDates(x.DateOfBirth, x.HireDate))
            .WithMessage("La fecha de ingreso debe ser posterior a que el empleado cumpla 18 años")
            .OverridePropertyName(nameof(RegisterEmployee.HireDate));
    }
    
    private bool NotContainSpaces(string username)
    {
        return !username.Contains(" ");
    }

    private bool CheckEmployeeAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            age--;
            
        return age >= 18 && age <= 65;
    }

    private bool ValidHireDate(DateTime hireDate)
    {
        return hireDate <= DateTime.Today;
    }
    
    private bool ReasonableHireDate(DateTime hireDate)
    {
        return hireDate >= DateTime.Today.AddYears(-50);
    }
    
    private bool ValidSalaryFormat(decimal salary)
    {
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(salary)[3])[2];
        return decimalPlaces <= 2;
    }
    
    private bool CoherentDates(DateTime dateOfBirth, DateTime hireDate)
    {
        var ageAtHire = hireDate.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > hireDate.AddYears(-ageAtHire))
            ageAtHire--;
            
        return ageAtHire >= 18;
    }
}