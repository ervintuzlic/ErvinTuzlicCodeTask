using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace IntusCodeTaskErvinTuzlic.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Window> Windows { get; set; }

    public DbSet<SubElement> SubElements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Windows)
            .WithOne(w => w.Order)
            .HasForeignKey(w => w.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Window>()
            .HasMany(w => w.SubElements)
            .WithOne(se => se.Window)
            .HasForeignKey(se => se.WindowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
