using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
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