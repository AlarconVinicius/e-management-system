using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.DomainObjects;

namespace EMS.WebApp.MVC.Data.Mappings;

public class ClientMapping : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

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

        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Number)
                .IsRequired()
                .HasMaxLength(Cpf.MaxCpfLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.MaxCpfLength})");
        });

        builder.HasOne(c => c.Address)
            .WithOne(c => c.Client);

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany(p => p.Clients)
            .HasForeignKey(c => c.CompanyId);
    }
}
