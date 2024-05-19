using ACB.Areas.Identity.Data;
using ACB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ACB.Data;

public class ACBContext : IdentityDbContext<ACBUser>
{
    public ACBContext(DbContextOptions<ACBContext> options)
        : base(options)
    {
    }

    public DbSet<ACB.Areas.Identity.Data.ACBUser>? ACBUsers { get; set; }

    public DbSet<NewOrder> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
