using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NzWalksDbContext _dbContext;

    public RegionsController(NzWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    // GET ALL REGIONS
    [HttpGet]
    public IActionResult GetAll()
    {
        // 1. Get Data From Database - Domain models 
        var regionsDomain = _dbContext.Regions.ToList();

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
    public IActionResult GetById([FromRoute] Guid id)
    {
        // 1. Get Data From Database - Domain models 
        var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
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
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // 1. Map or Convert DTO to Domain Model
        var regionDomain = new Region()
        {
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            Code = addRegionRequestDto.Code,
        };

        // 2. Adding new entity to db 
        _dbContext.Regions.Add(regionDomain);
        _dbContext.SaveChanges();

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
    public IActionResult Update([FromBody] UpdateRegionRequestDto updateRegionRequestDto, [FromRoute] Guid id)
    {
        // 1. Find Domain Model
        var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
        if (regionDomain == null) return NotFound();

        // 2. Update Domain Model Values
        regionDomain.Name = updateRegionRequestDto.Name;
        regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
        regionDomain.Code = updateRegionRequestDto.Code;

        // 3. Update Db 
        _dbContext.Update(regionDomain);
        _dbContext.SaveChanges();

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
    public IActionResult Delete([FromRoute] Guid id)
    {
        // 1. Find domain model region
        var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
        if (regionDomain == null) return NotFound();
        
        // 2. Remove it from database
        _dbContext.Regions.Remove(regionDomain);
        _dbContext.SaveChanges();
        
        // 3. Return no content
        return NoContent();
    }
}