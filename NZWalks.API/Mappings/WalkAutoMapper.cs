using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Mappings;

public class WalkAutoMapper : Profile
{
    public WalkAutoMapper()
    {
        CreateMap<Walk, AddWalkRequestDTO>().ReverseMap();
        CreateMap<Walk, UpdateWalkRequestDto>().ReverseMap();
        CreateMap<Walk, WalkDto>()
            .ForMember(x => x.DifficultyName,
                opt => opt.MapFrom(x => x.Difficulty.Name))
            .ForMember(x => x.RegionName,
                opt => opt.MapFrom(x => x.Region.Name));
    }
}