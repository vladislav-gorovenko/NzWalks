using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositaries;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository _regionRepository;

    public RegionsController(NzWalksDbContext dbContext, IRegionRepository regionRepository)
    {
        this._regionRepository = regionRepository;
    }

    // GET ALL REGIONS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // 1. Get Data From Database - Domain models 
        var regionsDomain = await _regionRepository.GetAllAsync();

        // 2. Convert Data to DTOs 
        var regionsDto = new List<RegionDto>();

        foreach (var regionDomain in regionsDomain)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
                Code = regionDomain.Code,
            });
        }

        // 3. Return DTOs 
        return Ok(regionsDto);
    }

    // GET SINGLE REGION BY ID
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // 1. Get Data From Database - Domain models 
        var regionDomain = await _regionRepository.GetById(id);
        if (regionDomain == null) return NotFound();

        // 2. Convert Data to DTOs 
        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl,
            Code = regionDomain.Code,
        };

        // 3. Return DTOs 
        return Ok(regionDto);
    }

    // CREATE NEW REGION
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // 1. Map or Convert DTO to Domain Model
        var regionDomain = new Region()
        {
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            Code = addRegionRequestDto.Code,
        };

        // 2. Adding new entity to db 
        await _regionRepository.Add(regionDomain);

        // 3. Map Domain Model back to DTO 
        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl,
            Code = regionDomain.Code,
        };
        return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
    }

    // UPDATE REGION
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateRegionRequestDto updateRegionRequestDto,
        [FromRoute] Guid id)
    {
        // 1. Find Domain Model
        var regionDomain = await _regionRepository.GetById(id);
        if (regionDomain == null) return NotFound();

        // 2. Update Domain Model Values & 3. Update Db 
        await _regionRepository.Update(regionDomain, updateRegionRequestDto);

        // 4. Map Domain Model back to DTO 
        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Code = regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl,
        };

        return Ok(regionDto);
    }

    // DELETE REGION
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // 1. Find domain model region
        var regionDomain = await _regionRepository.GetById(id);
        if (regionDomain == null) return NotFound();

        // 2. Remove it from database
        await _regionRepository.Delete(regionDomain);

        // 3. Return no content
        return NoContent();
    }
}