using FluentValidation;
using NZWalks.Core.Models.DTOs.Auth;

namespace NZWalks.Core.Validators.Auth;

public class SignupRequestValidator : AbstractValidator<SignupRequestDto>
{
    public SignupRequestValidator()
    {

        RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(30);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(30);
    }
}