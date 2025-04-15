using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositaries;

public class SQLWalkRepository : IWalkRepository
{
    private readonly NzWalksDbContext _dbContext;

    public SQLWalkRepository(NzWalksDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<Walk>> GetAllAsync()
    {
        return await _dbContext.Walks
            .Include(x => x.Difficulty)
            .Include(x => x.Region)
            .ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Walks
            .Include(x => x.Difficulty)
            .Include(x => x.Region)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Walk> CreateAsync(Walk walkDomain)
    {
        await _dbContext.Walks.AddAsync(walkDomain);
        await _dbContext.SaveChangesAsync();
        
        return walkDomain;
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walkDomain)
    {
        var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null) return null;

        existingWalk.Name = walkDomain.Name;
        existingWalk.Description = walkDomain.Description;
        existingWalk.LengthInKm = walkDomain.LengthInKm;
        existingWalk.WalkImageUrl = walkDomain.WalkImageUrl;
        existingWalk.RegionId = walkDomain.RegionId;
        existingWalk.DifficultyId = walkDomain.DifficultyId;

        await _dbContext.SaveChangesAsync();

        return existingWalk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var walkDomain = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (walkDomain == null) return null;

        _dbContext.Walks.Remove(walkDomain);
        await _dbContext.SaveChangesAsync();

        return walkDomain;
    }
}