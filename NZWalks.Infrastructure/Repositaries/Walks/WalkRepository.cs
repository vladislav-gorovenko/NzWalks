using Microsoft.EntityFrameworkCore;
using NZWalks.Application.Walks.Repositories;
using NZWalks.Domain.Models;
using NZWalks.Infrastructure.Data;

namespace NZWalks.Infrastructure.Repositaries.Walks;

public class WalkRepository : IWalkRepository
{
    private readonly NzWalksDbContext _dbContext;

    public WalkRepository(NzWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool? isDescending = false, int? skip = 0, int? top = null)
    {
        var query = _dbContext.Walks.AsQueryable();

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

        return await query
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