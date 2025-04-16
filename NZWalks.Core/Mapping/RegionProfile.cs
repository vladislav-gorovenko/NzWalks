using AutoMapper;
using NZWalks.Core.Models.Domain;
using NZWalks.Core.Models.DTOs.Region;

namespace NZWalks.Core.Mapping;

public class RegionProfile : Profile
{
    public RegionProfile()
    {
        CreateMap<RegionDto, Region>()
            // .ForMember(x => x.RegionImageUrl, opt => opt.MapFrom(x => x.RegionImageUrl))
            // .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            // .ForMember(x => x.Code, opt => opt.MapFrom(x => x.Code))
            // .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ReverseMap();
        CreateMap<AddRegionRequestDto, Region>()
            .ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>()
            .ReverseMap();
    }
}