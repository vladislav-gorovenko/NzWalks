using NZWalks.Core.Models.DTOs.Region;
using NZWalks.Domain.Models;

namespace NZWalks.Application.Regions.Profile;

public class RegionProfile : AutoMapper.Profile
{
    public RegionProfile()
    {
        CreateMap<RegionDto, Region>()
            .ReverseMap();
        CreateMap<AddRegionRequestDto, Region>()
            .ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>()
            .ReverseMap();
    }
}