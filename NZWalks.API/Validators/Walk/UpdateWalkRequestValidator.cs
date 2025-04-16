using FluentValidation;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Validators.Walk;

public class UpdateWalkRequestValidator : AbstractValidator<UpdateWalkRequestDto>
{
    public UpdateWalkRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(12);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LengthInKm).NotEmpty().GreaterThan(0);
        RuleFor(x => x.WalkImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.WalkImageUrl))
            .WithMessage("The field shall be either empty or valid URL");
        RuleFor(x => x.RegionId).NotEmpty();
        RuleFor(x => x.DifficultyId).NotEmpty();
    }
}