namespace NZWalks.Domain.Models;

public class Region : BaseEntity<Guid>
{
    public required string Code { get; set; }
    public string? RegionImageUrl { get; set; }
}