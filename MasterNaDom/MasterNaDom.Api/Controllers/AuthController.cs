using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MasterNaDom.Api.Data;
using MasterNaDom.Api.Models;

namespace MasterNaDom.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public AuthController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public class RegisterMasterDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal HourlyRate { get; set; }
        public string City { get; set; } = "Москва";
    }

    [HttpPost("register-master")]
    public async Task<IActionResult> RegisterMaster([FromBody] RegisterMasterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            IsMaster = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var profile = new MasterProfile
        {
            UserId = user.Id,
            Category = dto.Category,
            HourlyRate = dto.HourlyRate,
            City = dto.City
        };

        _db.MasterProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Мастер успешно зарегистрирован!" });
    }
}