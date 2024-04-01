using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data;

public class EMSDbContext : DbContext, IUnitOfWork
{
    public EMSDbContext(DbContextOptions<EMSDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EMSDbContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        //foreach (var entry in ChangeTracker.Entries()
        //        .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified))
        //{
        //    if (entry.State == EntityState.Added)
        //    {
        //        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
        //    }
        //    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        //}

        var sucesso = await base.SaveChangesAsync() > 0;
        return sucesso;
    }
}