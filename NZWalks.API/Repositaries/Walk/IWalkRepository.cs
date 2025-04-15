

using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositaries;

public interface IWalkRepository
{
    Task<List<Walk>> GetAllAsync();
    Task<Walk?> GetByIdAsync(Guid id);
    Task<Walk> CreateAsync(Walk walkDomain);
    Task<Walk?> UpdateAsync(Guid id, Walk walkDomain);
    Task<Walk?> DeleteAsync(Guid id);
}