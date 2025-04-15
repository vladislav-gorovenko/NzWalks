using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTOs;

public class AddWalkRequestDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid RegionId { get; set; }
    public Guid DifficultyId { get; set; }
}