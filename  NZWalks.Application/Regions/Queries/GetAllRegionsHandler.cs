using AutoMapper;
using MediatR;
using NZWalks.Application.Regions.Repositories;

namespace NZWalks.Application.Regions.Queries;

public class GetAllRegionsHandler : IRequestHandler<GetAllRegionsQuery, List<RegionDto>>
{
    private readonly IRegionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRegionsHandler(IRegionRepository regionRepository, IMapper mapper)
    {
        this._repository = regionRepository;
        this._mapper = mapper;
    }

    public async Task<List<RegionDto>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
    {
        var regionsDomain = await _repository.GetAllAsync(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsDescending, request.Skip, request.Top);
        return _mapper.Map<List<RegionDto>>(regionsDomain);
    }
}