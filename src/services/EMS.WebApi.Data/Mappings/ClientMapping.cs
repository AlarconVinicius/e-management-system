using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class ClientMapping : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasMany(c => c.ServiceAppointments)
               .WithOne(sa => sa.Client)
               .HasForeignKey(sa => sa.ClientId);
    }
}
