using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MasterNaDom.Api.Data;

namespace MasterNaDom.Api.Pages;

public class MastersModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public List<dynamic> Masters { get; set; } = new();

    public MastersModel(ApplicationDbContext db) => _db = db;

    public async Task OnGetAsync()
    {
        Masters = await _db.MasterProfiles
            .Include(m => m.User)
            .Where(m => m.City == "Москва")
            .Select(m => new { m.User!.FullName, m.Category, m.HourlyRate })
            .ToListAsync<dynamic>();
    }
}