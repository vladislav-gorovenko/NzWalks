using FluentValidation;
using NZWalks.Core.Models.DTOs.Auth;

namespace NZWalks.Core.Validators.Auth;

public class LoginRequestValidator: AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();;
    }
}