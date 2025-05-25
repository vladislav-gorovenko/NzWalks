namespace NZWalks.Application.Regions.Commands;

public class AddRegionCommand
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}