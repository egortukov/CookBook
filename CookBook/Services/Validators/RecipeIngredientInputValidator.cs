using CookBook.DTOs;
using FluentValidation;

namespace CookBook.Services.Validators;

public class RecipeIngredientInputValidator: AbstractValidator<RecipeIngredientInputDto>
{
    public RecipeIngredientInputValidator()
    {
        RuleFor(dto => dto.IngredientId)
            .GreaterThan(0)
            .WithMessage("Ингредиент должен быть выбран");

        RuleFor(dto => dto.Amount)
            .GreaterThan(0)
            .WithMessage("Количество должно быть больше 0");
    }
}
