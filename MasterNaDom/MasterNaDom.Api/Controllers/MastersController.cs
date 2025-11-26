using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterNaDom.Api.Data;

namespace MasterNaDom.Api.Controllers;

[Route("api/masters")]
[ApiController]
public class MastersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public MastersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string city = "Москва")
    {
        var masters = await _db.MasterProfiles
            .Where(m => m.City == city)
            .Select(m => new
            {
                m.Id,
                FullName = m.User!.FullName,
                m.Category,
                m.HourlyRate,
                m.City
            })
            .ToListAsync();

        return Ok(masters);
    }
}