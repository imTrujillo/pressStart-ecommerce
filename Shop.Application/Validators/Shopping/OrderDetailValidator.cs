using FluentValidation;
using Shop.Application.DTOs.Response.Shopping;

namespace Shop.Application.Validators.Shopping;

public class OrderDetailValidator : AbstractValidator<OrderDetailDto>
{
    public OrderDetailValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("El Id del Producto es Requerido")
            .GreaterThan(0).WithMessage("El Id del Producto debe ser mayor a 0");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("La cantidad del Producto es Requerida")
            .GreaterThan(0).WithMessage("La cantidad del Producto debe ser mayor a 0")
            .LessThan(50).WithMessage("La cantidad del producto no puede ser mayor a 50");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("El Precio del Producto es requerido")
            .GreaterThan(0).WithMessage("El Precio del producto debe ser mayor")
            .LessThan(50000).WithMessage("El Precio del Producto no puede ser mayor a $50,000");
    }
}