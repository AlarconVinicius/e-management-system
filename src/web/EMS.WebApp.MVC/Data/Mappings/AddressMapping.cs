using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApp.MVC.Data.Mappings;

public class AddressMapping : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Street)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Number)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(c => c.ZipCode)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(c => c.Complement)
            .HasColumnType("varchar(250)");

        builder.Property(c => c.Neighborhood)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.City)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.State)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.Property(a => a.UserId)
                .IsRequired(false);

        builder.Property(a => a.ClientId)
            .IsRequired(false);

        builder.HasOne(c => c.User)
            .WithOne(s => s.Address)
            .HasForeignKey<Address>(a => a.UserId);

        builder.HasOne(c => c.Client)
            .WithOne(s => s.Address)
            .HasForeignKey<Address>(a => a.ClientId);
    }
}
