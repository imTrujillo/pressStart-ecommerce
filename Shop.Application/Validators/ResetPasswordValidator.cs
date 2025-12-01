using FluentValidation;
using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;

namespace Shop.Application.Validators;

public class ResetPasswordValidator : AbstractValidator<ResetPassword>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El Nombre de Usuario es requerido")
            .Length(5, 50).WithMessage("El Nombre de Usuario debe tener entre 5 y 50 caracteres")
            .Must(NotContainSpaces).WithMessage("El nombre de usuario no puede contener espacios");

        //RuleFor(x => x.Dui)
        //    .NotEmpty().WithMessage("El DUI es requerido")
        //    .Must(DocumentsValidator.IsValidDui)
        //    .WithMessage("El formato del DUI es inválido. Formato correcto: 12345678-9");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La Contraseña es requerida")
            .MinimumLength(8).WithMessage("La Contraseña debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("La Contraseña debe tener al menos: Una Mayúscula, Una Minúscula, Un número, y Un Carácter Especial");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es requerida")
            .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");
    }
    
    private bool NotContainSpaces(string username)
    {
        return !username.Contains(" ");
    }
}