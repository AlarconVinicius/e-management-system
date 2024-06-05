using EMS.Core.DomainObjects;
using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class CompanyMapping : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.PlanId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Brand)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.OwnsOne(c => c.Document, tf =>
        {
            tf.Property(c => c.Number)
                .IsRequired()
                .HasMaxLength(Cpf.MaxCpfLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.MaxCpfLength})");
        });

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasOne(c => c.Plan)
            .WithMany(p => p.Companies)
            .HasForeignKey(c => c.PlanId);

        builder.HasMany(p => p.Users)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);

        builder.HasMany(p => p.Products)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);
    }
}