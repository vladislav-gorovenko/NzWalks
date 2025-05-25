using NZWalks.Domain.Models;

namespace NZWalks.Application.Regions.Repositories;

public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
        bool? isDescending = false, int? skip = 0, int? top = 10);

    Task<Region?> GetByIdAsync(Guid id);
    Task<Region> CreateAsync(Region regionDomain);
    Task<Region?> UpdateAsync(Guid id, Region regionDomain);
    Task<Region?> DeleteAsync(Guid id);
}