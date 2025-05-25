using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Application.Walks.Commands;
using NZWalks.Application.Walks.Queries;
using NZWalks.Application.Walks.Repositories;
using NZWalks.Domain.Models;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IWalkRepository _walkRepository;
    private readonly IMapper _mapper;

    public WalksController(IWalkRepository walkRepository, IMapper mapper)
    {
        _walkRepository = walkRepository;
        _mapper = mapper;
    }

    // GET ALL WALKS 
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isDescending, [FromQuery] int? skip, [FromQuery] int? top)
    {
        var walkDomain = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isDescending, skip, top);
        var walksDto = _mapper.Map<List<WalkDto>>(walkDomain);

        return Ok(walksDto);
    }

    // GET SINGLE WALK BY ID
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomain = await _walkRepository.GetByIdAsync(id);
        if (walkDomain == null) return NotFound();

        var walkDto = _mapper.Map<WalkDto>(walkDomain);

        return Ok(walkDto);
    }

    // CREATE NEW WALKS 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDTO)
    {
        var walkDomain = _mapper.Map<Walk>(addWalkRequestDTO);
        walkDomain = await _walkRepository.CreateAsync(walkDomain);

        var walkDto = _mapper.Map<WalkDto>(walkDomain);
        return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
    }

    // UPDATE WALK
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateWalkRequestDto updateWalkRequestDto,
        [FromRoute] Guid id)
    {
        var walkDomain = _mapper.Map<Walk>(updateWalkRequestDto);
        var currentWalk = await _walkRepository.UpdateAsync(id, walkDomain);
        if (currentWalk == null) return NotFound();

        var walkDto = _mapper.Map<WalkDto>(currentWalk);

        return Ok(walkDto);
    }

    // DELETE WALK
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedWalk = await _walkRepository.DeleteAsync(id);
        if (deletedWalk == null) return NotFound();

        return NoContent();
    }
}