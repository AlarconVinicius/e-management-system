using EMS.WebApp.MVC.Business.DomainObjects;
using EMS.WebApp.MVC.Business.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApp.MVC.Data.Mappings;

public class SubscriberMapping : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Number)
                .IsRequired()
                .HasMaxLength(Cpf.MaxCpfLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.MaxCpfLength})");
        });

        builder.OwnsOne(c => c.Email, tf =>
        {
            tf.Property(c => c.Address)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType($"varchar({Email.MaxAddressLength})");
        });


        // 1 : 1 => User : Address
        builder.HasOne(c => c.Address)
            .WithOne(c => c.Subscriber);

    }
}
