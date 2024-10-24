using Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace DataService.Data;

public class AppDbContext : DbContext
{
    public virtual DbSet<GeolocationData> Geolocations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GeolocationData>()
            .HasKey(b => b.Ip);

        modelBuilder.Entity<GeolocationData>(entity =>
        {
            entity.OwnsOne(d => d.Location);
        });
    }
}
