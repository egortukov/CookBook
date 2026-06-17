using CookBook.DTOs;
using FluentValidation;

namespace CookBook.Services.Validators;

public class CreateRecipeValidator : AbstractValidator<CreateRecipeDto>
{
    public CreateRecipeValidator()
    {
        RuleFor(dto => dto.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(dto => dto.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(2000);
        
        RuleFor(dto => dto.Ingredients)
            .NotNull()
            .NotEmpty()
            .WithMessage("Рецепт должен содержать хотя бы один ингредиент");

        RuleForEach(dto => dto.Ingredients)
            .SetValidator(new RecipeIngredientInputValidator());
    }
}