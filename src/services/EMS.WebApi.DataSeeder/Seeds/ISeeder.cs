using EMS.WebApi.Business.Models;

namespace EMS.WebApi.DataSeeder.Seeds;
public interface ISeeder
{
    Task<List<Client>> GenerateClients(Guid companyId);
    Task<Guid> GenerateDefaultCompany(Guid planId);
    Task<Employee> GenerateDefaultEmployee(Guid companyId);
    Task<List<Plan>> GenerateDefaultPlans();
    Task GenerateDefaultUser(Employee employee);
    Task<List<Employee>> GenerateEmployees(Guid companyId);
    Task GenerateFirstData();
    Task<List<ServiceAppointment>> GenerateServiceAppointments(Guid companyId, List<Service> services, List<Employee> employees, List<Client> clients);
    Task<List<Service>> GenerateServices(Guid companyId);
}