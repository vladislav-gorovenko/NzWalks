using MediatR;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Core.Models.Queries;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsMediatRController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegionsMediatRController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    // GET ALL REGIONS
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isDescending, [FromQuery] int? skip, [FromQuery] int? top)
    {
        var query = new GetAllRegionsQuery
        {
            FilterOn = filterOn,
            FilterQuery = filterQuery,
            SortBy = sortBy,
            IsDescending = isDescending,
            Skip = skip,
            Top = top
        };
        var regionsDto = await _mediator.Send(query);
        return Ok(regionsDto);
    }
}