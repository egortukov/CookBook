using CookBook.Filters;
using FluentValidation;

namespace CookBook.Validators;

public class RecipeFilterValidator : AbstractValidator<RecipeFilter>
{
    public RecipeFilterValidator()
    {
        RuleFor(f => f.MinRating)
            .InclusiveBetween(1, 5)
            .When(f => f.MinRating is not null);

        RuleFor(f => f.SortBy)
            .IsInEnum()
            .When(f => f.SortBy is not null);
    }
}