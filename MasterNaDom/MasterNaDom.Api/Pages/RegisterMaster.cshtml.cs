using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MasterNaDom.Api.Data;
using MasterNaDom.Api.Models;

namespace MasterNaDom.Api.Pages;

public class RegisterMasterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public RegisterMasterModel(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal HourlyRate { get; set; }
        public string City { get; set; } = "Москва";
    }

    public string? Message { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            FullName = Input.FullName,
            IsMaster = true
        };

        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            var profile = new MasterProfile
            {
                UserId = user.Id,
                Category = Input.Category,
                HourlyRate = Input.HourlyRate,
                City = Input.City
            };

            _db.MasterProfiles.Add(profile);
            await _db.SaveChangesAsync();

            Message = "Вы успешно зарегистрированы как мастер!";
            Input = new(); 
        }
        else
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}