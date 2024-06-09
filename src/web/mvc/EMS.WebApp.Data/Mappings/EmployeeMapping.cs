using EMS.WebApp.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.WebApp.Data.Mappings;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.Property(c => c.Salary)
            .HasDefaultValue(0.0)
            .HasColumnType("decimal(18, 2)");

        builder.ToTable(t => t.HasCheckConstraint("CK_Employee_Salary", "Salary >= 0"));
    }
}
