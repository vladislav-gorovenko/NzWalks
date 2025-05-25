using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Application.Regions.Commands;
using NZWalks.Application.Regions.Queries;
using NZWalks.Application.Regions.Repositories;
using NZWalks.Domain.Models;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionsController(IRegionRepository regionRepository, IMapper mapper)
    {
        this._regionRepository = regionRepository;
        this._mapper = mapper;
    }

    // GET ALL REGIONS
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isDescending, [FromQuery] int? skip, [FromQuery] int? top)
    {
        var regionsDomain = await _regionRepository.GetAllAsync(filterOn, filterQuery, sortBy, isDescending, skip, top);
        var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

        return Ok(regionsDto);
    }

    // GET SINGLE REGION BY ID
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var regionDomain = await _regionRepository.GetByIdAsync(id);
        if (regionDomain == null) return NotFound();

        var regionDto = _mapper.Map<RegionDto>(regionDomain);

        return Ok(regionDto);
    }

    // CREATE NEW REGION
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionCommand addRegionRequestDto)
    {
        var regionDomain = _mapper.Map<Region>(addRegionRequestDto);
        regionDomain = await _regionRepository.CreateAsync(regionDomain);

        var regionDto = _mapper.Map<RegionDto>(regionDomain);
        return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
    }

    // UPDATE REGION
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateRegionCommand updateRegionRequestDto,
        [FromRoute] Guid id)
    {
        var regionDomain = _mapper.Map<Region>(updateRegionRequestDto);
        var currentRegion = await _regionRepository.UpdateAsync(id, regionDomain);
        if (currentRegion == null) return NotFound();

        var regionDto = _mapper.Map<RegionDto>(currentRegion);

        return Ok(regionDto);
    }

    // DELETE REGION
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedRegion = await _regionRepository.DeleteAsync(id);
        if (deletedRegion == null) return NotFound();

        return NoContent();
    }
}