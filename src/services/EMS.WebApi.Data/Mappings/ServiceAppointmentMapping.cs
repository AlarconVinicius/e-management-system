using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class ServiceAppointmentMapping : IEntityTypeConfiguration<ServiceAppointment>
{
    public void Configure(EntityTypeBuilder<ServiceAppointment> builder)
    {
        builder.ToTable("ServiceAppointments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.EmployeeId)
            .IsRequired();

        builder.Property(c => c.ClientId)
            .IsRequired();

        builder.Property(c => c.ServiceId)
            .IsRequired();

        builder.Property(c => c.AppointmentStart)
            .IsRequired();

        builder.Property(c => c.AppointmentEnd)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired();
        //.HasConversion(
        //        v => v.ToString(),
        //        v => (EServiceStatus)Enum.Parse(typeof(EServiceStatus), v)
        //    );

        builder.HasOne(c => c.Company)
               .WithMany(p => p.ServiceAppointments)
               .HasForeignKey(c => c.CompanyId);

        builder.HasOne(c => c.Employee)
               .WithMany(sa => sa.ServiceAppointments)
               .HasForeignKey(sa => sa.EmployeeId);

        builder.HasOne(c => c.Client)
               .WithMany(sa => sa.ServiceAppointments)
               .HasForeignKey(sa => sa.ClientId);

        builder.HasOne(c => c.Service)
               .WithMany(sa => sa.ServiceAppointments)
               .HasForeignKey(sa => sa.ClientId);
    }
}