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
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> CreateAsync(Region regionDomain)
    {
        await _dbContext.Regions.AddAsync(regionDomain);
        await _dbContext.SaveChangesAsync();
        return regionDomain;
    }

    public async Task<Region?> UpdateAsync(Guid id, UpdateRegionRequestDto regionDto)
    {
        var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (regionDomain == null) return null;

        regionDomain.Name = regionDto.Name;
        regionDomain.RegionImageUrl = regionDto.RegionImageUrl;
        regionDomain.Code = regionDto.Code;

        await _dbContext.SaveChangesAsync();

        return regionDomain;
    }

    public async Task<Region?> DeleteAsync(Guid id)
    {
        var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (regionDomain == null) return null;

        _dbContext.Regions.Remove(regionDomain);
        await _dbContext.SaveChangesAsync();
        
        return regionDomain;
    }
}