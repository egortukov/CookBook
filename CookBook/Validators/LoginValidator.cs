using CookBook.DTOs;
using FluentValidation;

namespace CookBook.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
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