using EMS.Core.DomainObjects;
using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.UseTptMappingStrategy();
        builder.ToTable("Users");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Role)
            .IsRequired()
            .HasColumnType("SMALLINT");

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.OwnsOne(c => c.Email, tf =>
        {
            tf.Property(c => c.Address)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType($"varchar({Email.MaxAddressLength})");
        });

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasColumnType("varchar(15)");

        builder.OwnsOne(c => c.Document, tf =>
        {
            tf.Property(c => c.Number)
                .IsRequired()
                .HasMaxLength(Cpf.MaxCpfLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.MaxCpfLength})");
        });

        //builder.HasDiscriminator<string>("Discriminator")
        //        .HasValue<Employee>("Employee")
        //        .HasValue<Client>("Client");

        builder.HasOne(c => c.Address)
            .WithOne(c => c.User);

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany(p => p.Users)
            .HasForeignKey(c => c.CompanyId);
    }
}
