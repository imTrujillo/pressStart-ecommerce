using FluentValidation;
using Shop.Application.DTOs.Request.Shopping;

namespace Shop.Application.Validators.Shopping;

public class OrderValidator : AbstractValidator<CreateOrderDto>
{
    public OrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El Id del Usuario es Requerido")
            .GreaterThan(0).WithMessage("El Id del Usuario debe ser mayor a 0");
        
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La Dirección es requerida")
            .Length(20, 200).WithMessage("La Dirección debe tener entre 20 y 200 caracteres");

        RuleForEach(x => x.Details).SetValidator(new OrderDetailValidator());
    }
}