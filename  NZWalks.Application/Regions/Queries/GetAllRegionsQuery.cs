using MediatR;

namespace NZWalks.Application.Regions.Queries;

public class GetAllRegionsQuery : IRequest<List<RegionDto>>
{
    public string? FilterOn { get; set; }
    public string? FilterQuery { get; set; }
    public string? SortBy { get; set; }
    public bool? IsDescending { get; set; }
    public int? Skip { get; set; }
    public int? Top { get; set; }
}