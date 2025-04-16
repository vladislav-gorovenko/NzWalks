using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositaries;

public class SQLRegionRepository : IRegionRepository
{
    private readonly NzWalksDbContext _dbContext;

    public SQLRegionRepository(NzWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool? isDescending = false, int? skip = 0, int? top = null)
    {
        var query = _dbContext.Regions.AsQueryable();

        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(r => r.Name.ToLower().Contains(filterQuery.ToLower()));
            }
        }

        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                query = isDescending == true ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name);
            }
        }

        query = query.Skip(skip ?? 0);
        if (top.HasValue && top.Value > 0)
        {
            query = query.Take(top.Value);
        }

        return await query.ToListAsync();
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

    public async Task<Region?> UpdateAsync(Guid id, Region regionDomain)
    {
        var currentRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (currentRegion == null) return null;

        currentRegion.Name = regionDomain.Name;
        currentRegion.RegionImageUrl = regionDomain.RegionImageUrl;
        currentRegion.Code = regionDomain.Code;

        await _dbContext.SaveChangesAsync();

        return currentRegion;
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