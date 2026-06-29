using CookBook.DTOs;
using FluentValidation;

namespace CookBook.Validators;

public class AddRatingValidator : AbstractValidator<AddRatingDto>
{
    public AddRatingValidator()
    {
        RuleFor(dto => dto.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Оценка должна быть от 1 до 5");
    }
}
