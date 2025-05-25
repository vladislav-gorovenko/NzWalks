using AutoMapper;
using NZWalks.Application.Regions.Commands;
using NZWalks.Application.Regions.Queries;
using NZWalks.Domain.Models;

namespace NZWalks.Application.Regions.Mappers;

public class RegionsMapper : Profile
{
    public RegionsMapper()
    {
        CreateMap<RegionDto, Region>()
            .ReverseMap();
        CreateMap<AddRegionCommand, Region>()
            .ReverseMap();
        CreateMap<UpdateRegionCommand, Region>()
            .ReverseMap();
    }
}