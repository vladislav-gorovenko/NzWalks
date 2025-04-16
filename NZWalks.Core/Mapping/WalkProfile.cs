using AutoMapper;
using NZWalks.Core.Models.Domain;
using NZWalks.Core.Models.DTOs.Walk;

namespace NZWalks.Core.Mapping;

public class WalkProfile : Profile
{
    public WalkProfile()
    {
        CreateMap<Walk, AddWalkRequestDto>().ReverseMap();
        CreateMap<Walk, UpdateWalkRequestDto>().ReverseMap();
        CreateMap<Walk, WalkDto>()
            .ForMember(x => x.DifficultyName,
                opt => opt.MapFrom(x => x.Difficulty.Name))
            .ForMember(x => x.RegionName,
                opt => opt.MapFrom(x => x.Region.Name));
    }
}