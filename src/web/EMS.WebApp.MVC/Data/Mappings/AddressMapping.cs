using EMS.WebApp.MVC.Business.Models.Users;
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

        // 1 : 1 => User : Address
        builder.HasOne(c => c.Subscriber)
            .WithOne(s => s.Address)
            .HasForeignKey<Address>(a => a.UserId);

        //// 1 : 1 => Employee : Address
        //builder.HasOne<Employee>(e => e.Employee)
        //    .WithOne(a => a.Address)
        //    .HasForeignKey<Address>(a => a.Id);

        //// 1 : 1 => Subscriber : Address
        //builder.HasOne(s => s.Subscriber)
        //    .WithOne(a => a.Address)
        //    .HasForeignKey<Address>(a => a.Id);

    }
}
