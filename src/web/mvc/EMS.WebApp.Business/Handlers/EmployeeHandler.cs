using EMS.Core.Handlers;
using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Mappings;
using EMS.WebApp.Business.Notifications;
using EMS.WebApp.Business.Services;
namespace EMS.WebApp.Business.Handlers;
public class EmployeeHandler : MainService//, IEmployeeHandler
{
    public readonly IEmployeeRepository _employeeRepository;

    public EmployeeHandler(INotifier notifier, IEmployeeRepository employeeRepository) : base(notifier)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Response<EmployeeResponse>> GetByIdAsync(GetEmployeeByIdRequest request)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);

            return employee is null
                ? new Response<EmployeeResponse>(null, 200, "Colaborador não encontrado.")
                : new Response<EmployeeResponse>(employee.MapEmployeeToEmployeeResponse());
        }
        catch
        {
            return new Response<EmployeeResponse>(null, 500, "Não foi possível recuperar o colaborador.");
        }
    }

    //public async Task<PagedResponse<List<EmployeeResponse>>> GetAllAsync(GetAllEmployeesRequest request)
    //{
    //    try
    //    {
    //        var employees = await _employeeRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, request.Query);

    //        return new PagedResponse<List<EmployeeResponse>>(
    //            employees.List.Select(x => x.MapEmployeeToEmployeeResponse()).ToList(),
    //            employees.TotalResults,
    //            employees.PageIndex,
    //            employees.PageSize);
    //    }
    //    catch
    //    {
    //        return new PagedResponse<List<EmployeeResponse>>(null, 500, "Não foi possível consultar os colaboradores.");
    //    }
    //}

    public async Task<Response<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (IsCpfInUse(request.Document, request.CompanyId)) return new Response<EmployeeResponse>(null, 400, "Não foi possível criar o colaborador."); ;
        var employeeMapped = request.MapCreateEmployeeRequestToEmployee();
        try
        {
            await _employeeRepository.AddAsync(employeeMapped);
            return new Response<EmployeeResponse>(employeeMapped.MapEmployeeToEmployeeResponse(), 201, "Colaborador criado com sucesso!");
        }
        catch
        {
            return new Response<EmployeeResponse>(null, 500, "Não foi possível criar o colaborador.");
        }
    }

    public async Task<Response<EmployeeResponse>> UpdateAsync(UpdateEmployeeRequest request)
    {
        //if (!ExecuteValidation(new EmployeeValidation(), employee)) return;
        if (!await UserExists(request.Id)) return null;
        var employeeDb = await _employeeRepository.GetByIdAsync(request.Id);

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

            return new Response<EmployeeResponse>(null, 204, "Colaborador atualizado com sucesso!");
        }
        catch
        {
            return new Response<EmployeeResponse>(null, 500, "Não foi possível atualizar o colaborador.");
        }
    }

    public async Task<Response<EmployeeResponse>> DeleteAsync(DeleteEmployeeRequest request)
    {
        try
        {
            if (!await UserExists(request.Id)) return null;

            await _employeeRepository.DeleteAsync(request.Id);

            return new Response<EmployeeResponse>(null, 204, "Colaborador deletado com sucesso!");
        }
        catch
        {
            return new Response<EmployeeResponse>(null, 500, "Não foi possível deletar o colaborador.");
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
