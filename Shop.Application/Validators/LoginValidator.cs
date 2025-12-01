using FluentValidation;
using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;

namespace Shop.Application.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El Nombre de Usuario es requerido")
            .Length(5, 50).WithMessage("El Nombre de Usuario debe tener entre 5 y 50 caracteres")
            .Must(NotContainSpaces).WithMessage("El nombre de usuario no puede contener espacios");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La Contraseña es requerida");
    }
    
    private bool NotContainSpaces(string username)
    {
        return !username.Contains(" ");
    }
}