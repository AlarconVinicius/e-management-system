using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Models.Subscription;
using EMS.WebApp.MVC.Business.Models.Users;
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

    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanSubscriber> PlanSubscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        //foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        //    .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EMSDbContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var sucesso = await base.SaveChangesAsync() > 0;
        return sucesso;
    }
}