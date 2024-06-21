using EMS.WebApi.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApi.Data.Mappings;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.Property(c => c.Salary)
            .HasDefaultValue(0.0)
            .HasColumnType("decimal(18, 2)");

        builder.HasMany(c => c.ServiceAppointments)
               .WithOne(sa => sa.Employee)
               .HasForeignKey(sa => sa.EmployeeId);

        builder.ToTable(t => t.HasCheckConstraint("CK_Employee_Salary", "Salary >= 0"));
    }
}
