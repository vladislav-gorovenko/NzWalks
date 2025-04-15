using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositaries;

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
    public async Task<IActionResult> GetAll()
    {
        var regionsDomain = await _regionRepository.GetAllAsync();
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
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomain = _mapper.Map<Region>(addRegionRequestDto);
        regionDomain = await _regionRepository.CreateAsync(regionDomain);
        
        var regionDto = _mapper.Map<RegionDto>(regionDomain);
        return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
    }

    // UPDATE REGION
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateRegionRequestDto updateRegionRequestDto,
        [FromRoute] Guid id)
    {
        var regionDomain = await _regionRepository.UpdateAsync(id, updateRegionRequestDto);
        if (regionDomain == null) return NotFound();
        
        var regionDto = _mapper.Map<RegionDto>(regionDomain);

        return Ok(regionDto);
    }

    // DELETE REGION
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _regionRepository.DeleteAsync(id);
        
        return NoContent();
    }
}