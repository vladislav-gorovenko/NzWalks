namespace NZWalks.Core.Models.DTOs.Walk;

public class WalkDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid RegionId { get; set; }
    public Guid DifficultyId { get; set; }
    public required string RegionName { get; set; }
    public required string DifficultyName { get; set; }
}