using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositaries;

public class SQLRegionRepository : IRegionRepository
{
    private readonly NzWalksDbContext _dbContext;

    public SQLRegionRepository(NzWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<Region>> GetAllAsync()
    {
        var regionsDomain = await _dbContext.Regions.ToListAsync();
        return regionsDomain;
    }

    public async Task<Region?> GetById(Guid id)
    {
        var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        return regionDomain;
    }

    public async Task Add(Region regionDomain)
    {
        await _dbContext.Regions.AddAsync(regionDomain);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Region regionDomain, UpdateRegionRequestDto regionDto)
    {
        regionDomain.Name = regionDto.Name;
        regionDomain.RegionImageUrl = regionDto.RegionImageUrl;
        regionDomain.Code = regionDto.Code;

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Region regionDomain)
    {
        _dbContext.Regions.Remove(regionDomain);
        await _dbContext.SaveChangesAsync();
    }
}