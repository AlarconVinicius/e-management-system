namespace EMS.WebApi.Business.Models;
public class Employee : User
{
    public decimal Salary { get; set; }

    public Employee()
    {
        
    }
    public Employee(Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role = ERole.Employee, decimal salary = 0.0m) : base(companyId, name, lastName, email, phoneNumber, document, role)
    {
        Salary = salary;
    }
    public Employee(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, ERole role = ERole.Employee, decimal salary = 0.0m) : base(id, companyId, name, lastName, email, phoneNumber, document, role)
    {
        Salary = salary;
    }
}
