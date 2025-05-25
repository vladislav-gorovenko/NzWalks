using NZWalks.Application.Walks.Commands;
using NZWalks.Application.Walks.Queries;
using NZWalks.Domain.Models;

namespace NZWalks.Application.Walks.Mappers;

public class WalksMapper : AutoMapper.Profile
{
    public WalksMapper()
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