using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MasterNaDom.Api.Models;

namespace MasterNaDom.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<MasterProfile> MasterProfiles => Set<MasterProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<MasterProfile>(e =>
        {
            e.HasIndex(x => x.City);
            e.HasIndex(x => x.Category);
        });
    }
}