using FluentValidation;
using Shop.Application.DTOs.Request.Shopping;

namespace Shop.Application.Validators.Shopping;

public class ProductValidator : AbstractValidator<CreateProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("El Nombre de la categoría no puede estar vacío")
            .Length(6, 50).WithMessage("El Nombre de la categoría debe tener mínimo 8 carácteres y máximo 50");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("El Descripción de la categoría no puede estar vacío")
            .Length(20, 200).WithMessage("La Descripción de la categoría debe tener mínimo 20 carácteres y máximo 200");

        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage("El Stock es requerido")
            .GreaterThan(0).WithMessage("El Stock debe ser mayor a 0");
        
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("El Precio del Producto es requerido")
            .GreaterThan(0).WithMessage("El Precio del producto debe ser mayor")
            .LessThan(50000).WithMessage("El Precio del Producto no puede ser mayor a 50,000");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("El Id del Usuario es Requerido")
            .GreaterThan(0).WithMessage("El Id del Usuario debe ser mayor a 0");
        
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("El Id del Usuario es Requerido")
            .GreaterThan(0).WithMessage("El Id del Usuario debe ser mayor a 0");
    }
}