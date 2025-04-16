using AutoMapper;
using MediatR;
using NZWalks.API.Repositaries.Regions;
using NZWalks.Core.Models.DTOs.Region;
using NZWalks.Core.Models.Queries;

namespace NZWalks.API.Handlers;

public class GetAllRegionsHandler : IRequestHandler<GetAllRegionsQuery, List<RegionDto>>
{
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public GetAllRegionsHandler(IRegionRepository regionRepository, IMapper mapper)
    {
        this._regionRepository = regionRepository;
        this._mapper = mapper;
    }

    public async Task<List<RegionDto>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
    {
        var regionsDomain = await _regionRepository.GetAllAsync(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsDescending, request.Skip, request.Top);
        return _mapper.Map<List<RegionDto>>(regionsDomain);
    }
}