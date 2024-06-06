using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;
using EMS.WebApi.Business.Services;
using EMS.WebApi.Business.Utils;

namespace EMS.WebApi.Business.Handlers;

public class EmployeeHandler : MainService, IEmployeeHandler
{
    public readonly IEmployeeRepository _employeeRepository;
    public readonly ICompanyRepository _companyRepository;

    public EmployeeHandler(INotifier notifier, IAspNetUser appUser, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<EmployeeResponse> GetByIdAsync(GetEmployeeByIdRequest request)
    {
        if (TenantIsEmpty()) return null;

        try
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id, TenantId);

            if(employee is null)
            {
                Notify("Colaborador não encontrado.");
                return null;
            }
            return employee.MapEmployeeToEmployeeResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o colaborador.");
            return null;
        }
    }

    public async Task<PagedResponse<EmployeeResponse>> GetAllAsync(GetAllEmployeesRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            return (await _employeeRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, TenantId, request.Query)).MapPagedEmployeesToPagedResponseEmployees();
        }
        catch
        {
            Notify("Não foi possível consultar os colaboradores.");
            return null;
        }
    }

    public async Task CreateAsync(CreateEmployeeRequest request)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (TenantIsEmpty()) return;
        if (IsCpfInUse(request.Document, TenantId)) return;
        if (!CompanyExists(TenantId)) return;

        request.CompanyId = TenantId;
        var employeeMapped = request.MapCreateEmployeeRequestToEmployee();
        try
        {
            await _employeeRepository.AddAsync(employeeMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o colaborador.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateEmployeeRequest request)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (TenantIsEmpty()) return;
        if (!UserExists(request.Id, TenantId)) return;
        var employeeDb = await _employeeRepository.GetByIdAsync(request.Id, TenantId);

        try
        {
            if (request.Email != employeeDb.Email.Address)
            {
                employeeDb.SetEmail(request.Email);
            }

            employeeDb.SetName(request.Name);
            employeeDb.SetLastName(request.LastName);
            employeeDb.SetPhoneNumber(request.PhoneNumber);
            employeeDb.SetIsActive(request.IsActive);

            await _employeeRepository.UpdateAsync(employeeDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o colaborador.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteEmployeeRequest request)
    {
        if (TenantIsEmpty()) return;
        try
        {
            if (!UserExists(request.Id, TenantId)) return;

            await _employeeRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o colaborador.");
            return;
        }
    }

    private bool IsCpfInUse(string cpf, Guid companyId)
    {
        if (_employeeRepository.SearchAsync(f => f.Document.Number == cpf && f.CompanyId == companyId).Result.Any())
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }

    private bool UserExists(Guid id, Guid companyId)
    {
        if (_employeeRepository.SearchAsync(f => f.Id == id && f.CompanyId == companyId).Result.Any())
        {
            return true;
        };

        Notify("Usuário não encontrado.");
        return false;
    }

    private bool CompanyExists(Guid companyId)
    {
        if (_companyRepository.GetByIdAsync(companyId).Result is not null)
        {
            return true;
        };

        Notify("TenantId não encontrado.");
        return false;
    }

}
