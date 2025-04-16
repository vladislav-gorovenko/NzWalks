using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositaries;

public interface IWalkRepository
{
    Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
        bool? isDescending = false, int? skip = 0, int? top = 10);

    Task<Walk?> GetByIdAsync(Guid id);
    Task<Walk> CreateAsync(Walk walkDomain);
    Task<Walk?> UpdateAsync(Guid id, Walk walkDomain);
    Task<Walk?> DeleteAsync(Guid id);
}