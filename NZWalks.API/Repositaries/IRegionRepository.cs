using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositaries;

public interface IRegionRepository
{
     Task<List<Region>> GetAllAsync();
     Task<Region?> GetByIdAsync(Guid id);
     Task<Region> CreateAsync(Region regionDomain);
     Task<Region?> UpdateAsync(Guid id, UpdateRegionRequestDto regionDto);
     Task<Region?> DeleteAsync(Guid id);
}