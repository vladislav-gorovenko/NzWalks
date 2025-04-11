using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositaries;

public interface IRegionRepository
{
     Task<List<Region>> GetAllAsync();
     Task<Region?> GetById(Guid id);
     Task Add(Region regionDomain);
     Task Update(Region regionDomain, UpdateRegionRequestDto regionDto);
     Task Delete(Region regionDomain);
}