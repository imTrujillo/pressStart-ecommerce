using FluentValidation;
using Shop.Application.DTOs.Request.Shopping;

namespace Shop.Application.Validators.Shopping;

public class CategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El Nombre de la categoría no puede estar vacío")
            .Length(6, 50).WithMessage("El Nombre de la categoría debe tener mínimo 8 carácteres y máximo 50");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("El Descripción de la categoría no puede estar vacío")
            .Length(20, 200).WithMessage("La Descripción de la categoría debe tener mínimo 20 carácteres y máximo 200");
    }
}