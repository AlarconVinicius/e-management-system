using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Title)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Description)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        builder.Property(c => c.UnitaryValue)
            .HasDefaultValue(0.0)
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany(p => p.Products)
            .HasForeignKey(c => c.CompanyId);

        builder.HasOne(c => c.Tenant)
            .WithMany(p => p.Products)
            .HasForeignKey(c => c.TenantId);

        builder.ToTable(t => t.HasCheckConstraint("CK_Product_UnitaryValue", "UnitaryValue >= 0"));
    }
}
