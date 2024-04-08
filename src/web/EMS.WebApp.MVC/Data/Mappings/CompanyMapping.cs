using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Data.Mappings;

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

        builder.OwnsOne(c => c.CpfOrCnpj, tf =>
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

        builder.HasOne(c => c.Tenant)
            .WithOne(t => t.Company)
            .HasForeignKey<Company>(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        //builder.HasOne(c => c.Tenant)
        //    .WithMany(p => p.Companies)
        //    .HasForeignKey(c => c.TenantId)
        //    .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Users)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);

        builder.HasMany(p => p.Clients)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);

        builder.HasMany(p => p.Products)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);

        builder.HasMany(p => p.Services)
        .WithOne(c => c.Company)
        .HasForeignKey(c => c.CompanyId);
    }
}