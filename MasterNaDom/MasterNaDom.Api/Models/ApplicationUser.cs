using Microsoft.AspNetCore.Identity;

namespace MasterNaDom.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsMaster { get; set; } = false;
}