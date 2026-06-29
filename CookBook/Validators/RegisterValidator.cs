using CookBook.DTOs;
using FluentValidation;

namespace CookBook.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(dto => dto.Login)
            .NotNull()
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(dto => dto.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}