using Cloud_Assignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cloud_Assignment.Models;
namespace Cloud_Assignment.Data;

public class Cloud_AssignmentContext : IdentityDbContext<Cloud_AssignmentUser>
{
    public Cloud_AssignmentContext(DbContextOptions<Cloud_AssignmentContext> options)
        : base(options)
    {
    }
    public DbSet<FoodRecord> FoodRecord { get; set; }

    public DbSet<FinancialRecord> FinancialRecord { get; set; }
    public DbSet<InventoryRecord> InventoryRecord { get; set; }

    public DbSet<DistributionSchedule> DistributionSchedule { get; set; }
    public DbSet<RequestRecord> RequestRecord { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
