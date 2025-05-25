namespace NZWalks.Domain.Models;

public class Walk : BaseEntity<Guid>
{
    public required string Description { get; set; }
    public required double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public Guid RegionId { get; set; }
    public required Region Region { get; set; }
    public Guid DifficultyId { get; set; }
    public required Difficulty Difficulty { get; set; }
}