using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class ServiceMapping : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Duration)
            .IsRequired();

        builder.Property(c => c.Price)
            .HasDefaultValue(0.0)
            .HasColumnType("decimal(18, 2)");

        builder.HasOne(c => c.Company)
            .WithMany(p => p.Services)
            .HasForeignKey(c => c.CompanyId);

        builder.HasMany(c => c.ServiceAppointments)
               .WithOne(sa => sa.Service)
               .HasForeignKey(sa => sa.ServiceId);

        builder.ToTable(t => t.HasCheckConstraint("CK_Service_Price", "Price >= 0"));
    }
}