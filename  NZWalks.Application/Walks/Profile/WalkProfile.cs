using NZWalks.Core.Models.DTOs.Walk;
using NZWalks.Domain.Models;

namespace NZWalks.Application.Walks.Profile;

public class WalkProfile : AutoMapper.Profile
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