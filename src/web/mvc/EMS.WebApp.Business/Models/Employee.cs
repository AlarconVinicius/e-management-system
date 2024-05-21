namespace EMS.WebApp.Business.Models;
public class Employee : User
{
    public decimal Salary { get; set; }

    public Employee()
    {
        
    }
    public Employee(Guid id, Guid companyId, string name, string lastName, string email, string phoneNumber, string document, string role, decimal salary = 0.0m) : base(id, companyId, name, lastName, email, phoneNumber, document, role)
    {
        Salary = salary;
    }
}
