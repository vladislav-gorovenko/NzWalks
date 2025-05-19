namespace NZWalks.Core.Models.DTOs.Region;

public class AddRegionRequestDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}