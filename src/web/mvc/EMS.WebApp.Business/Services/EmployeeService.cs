using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;

namespace EMS.WebApp.Business.Services;

public class EmployeeService : MainService, IEmployeeService
{
    public readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(INotifier notifier, IEmployeeRepository employeeRepository) : base(notifier)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task Add(Employee employee)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (IsCpfInUse(employee.Id, employee.Document.Number)) return;

        await _employeeRepository.AddAsync(employee);
        return;
    }

    public async Task Update(Employee employee)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (!await UserExists(employee.Id)) return;

        var employeeDb = await _employeeRepository.GetByIdAsync(employee.Id);

        if (employee.Id != employeeDb.Id && employeeDb.Role != ERole.Admin)
        {
            Notify("Usuário não encontrado.");
            return;
        }


        if (employee.Email.Address != employeeDb.Email.Address)
        {
            employeeDb.SetEmail(employee.Email.Address);
        }

        employeeDb.SetName(employee.Name);
        employeeDb.SetLastName(employee.LastName);
        employeeDb.SetPhoneNumber(employee.PhoneNumber);

        await _employeeRepository.UpdateAsync(employeeDb);

        return;
    }

    public async Task Delete(Guid id)
    {
        if (!await UserExists(id)) return;

        await _employeeRepository.DeleteAsync(id);

        return;
    }

    private bool IsCpfInUse(Guid id, string document)
    {
        if (_employeeRepository.SearchAsync(f => f.Document.Number == document && f.Id != id).Result.Any())
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }

    private async Task<bool> UserExists(Guid id)
    {
        var userExist = await _employeeRepository.GetByIdAsync(id);

        if (userExist != null)
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }
}
