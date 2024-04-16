using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Mappings;

public class TenantMapping : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.IsActive)
               .IsRequired();

        builder.HasOne(t => t.Company)
            .WithOne(c => c.Tenant)
            .HasForeignKey<Company>(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        //builder.HasMany(t => t.Companies)
        //       .WithOne(c => c.Tenant)
        //       .HasForeignKey(c => c.TenantId)
        //       .OnDelete(DeleteBehavior.Restrict);
        //.OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Users)
        .WithOne(c => c.Tenant)
        .HasForeignKey(c => c.TenantId);
            //.OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Clients)
               .WithOne(c => c.Tenant)
               .HasForeignKey(c => c.TenantId);
               //.OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Products)
               .WithOne(c => c.Tenant)
               .HasForeignKey(c => c.TenantId);
               //.OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Services)
               .WithOne(c => c.Tenant)
               .HasForeignKey(c => c.TenantId); 
               //.OnDelete(DeleteBehavior.Cascade);
    }
}
