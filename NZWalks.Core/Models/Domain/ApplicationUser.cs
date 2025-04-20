using Microsoft.AspNetCore.Identity;

namespace NZWalks.Core.Models.Domain;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}