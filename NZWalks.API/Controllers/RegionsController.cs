using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

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
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var regions = _dbContext.Regions.ToList();
        return Ok(regions);
    }
}