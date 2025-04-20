using FluentValidation;
using NZWalks.Core.Models.Domain;

namespace NZWalks.Core.Validators.Auth;

public class TokenInfoValidator : AbstractValidator<TokenInfo>
{
    public TokenInfoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(30);
        RuleFor(x => x.RefreshToken).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ExpiredAt).NotEmpty();
    }
}