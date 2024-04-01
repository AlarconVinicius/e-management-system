using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Mappings;

public class ServiceMapping : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Title)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Description)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        builder.Property(c => c.Value)
            .HasDefaultValue(0.0)
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt) .IsRequired();
        builder.Property(p => p.UpdatedAt) .IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany(p => p.Services)
            .HasForeignKey(c => c.CompanyId);

        builder.ToTable(t => t.HasCheckConstraint("CK_Service_Value", "Value >= 0"));
    }
}
