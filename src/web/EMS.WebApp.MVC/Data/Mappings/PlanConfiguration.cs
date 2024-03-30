using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Subscription.API.Data.Mappings;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.Property(p => p.Subtitle)
            .IsRequired()
            .HasColumnType("varchar(400)");

        builder.Property(p => p.Price)
            .IsRequired();

        builder.Property(p => p.Benefits)
            .IsRequired()
            .HasColumnType("Text");

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasMany(p => p.Companies)
        .WithOne(c => c.Plan)
        .HasForeignKey(c => c.PlanId);
    }
}