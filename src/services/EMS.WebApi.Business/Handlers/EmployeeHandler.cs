﻿using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Employees;
using EMS.Core.Requests.Identities;
using EMS.Core.Responses;
using EMS.Core.Responses.Employees;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;

namespace EMS.WebApi.Business.Handlers;

public class EmployeeHandler : BaseHandler, IEmployeeHandler
{
    public readonly IEmployeeRepository _employeeRepository;
    public readonly IServiceAppointmentRepository _serviceAppointmentRepository;
    public readonly ICompanyRepository _companyRepository;
    public readonly IIdentityHandler _identityHandler;

    public EmployeeHandler(INotifier notifier, IAspNetUser appUser, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IIdentityHandler identityHandler, IServiceAppointmentRepository serviceAppointmentRepository) : base(notifier, appUser)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _identityHandler = identityHandler;
        _serviceAppointmentRepository = serviceAppointmentRepository;
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

    public async Task CreateAsync(CreateEmployeeAndUserRequest request)
    {
        //if (!ExecuteValidation(new CompanyValidation(), company)) return;
        if (TenantIsEmpty()) return;
        if (!CompanyExists(TenantId)) return;
        if (IsCpfInUse(request.Employee.Document, TenantId)) return;

        var userId = Guid.NewGuid();
        request.Employee.Id = userId;
        request.User.Id = userId;
        request.Employee.CompanyId = TenantId;
        request.Employee.Role = Core.Enums.ERoleCore.Employee;
        var employyeeMapped = request.Employee.MapCreateEmployeeRequestToEmployee();
        try
        {
            await _employeeRepository.AddAsync(employyeeMapped);
            var result = await _identityHandler.CreateAsync(request.User);
            if (!IsOperationValid())
            {
                await _employeeRepository.DeleteAsync(userId);
                return;
            };
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

    public async Task UpdateAsync(UpdateEmployeeAndUserRequest request)
    {
        try
        {
            await UpdateAsync(request.Employee);
            if (IsOperationValid())
            {
                var identityUser = await _identityHandler.GetByIdAsync(new GetUserByIdRequest(request.Employee.Id));
                if (identityUser is null)
                {
                    ClearNotifications();
                } 
                else
                {
                    await _identityHandler.UpdateEmailAsync(request.User);
                }
                return;
            };
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
            //await _serviceAppointmentRepository.UpdateEmployeeIdAsync(request.Id, request.NewEmployeeId);
            if (IsOperationValid())
            {
                //await _employeeRepository.DeleteAsync(request.Id);
                var identityUser = await _identityHandler.GetByIdAsync(new GetUserByIdRequest(request.Id));
                //if (identityUser is null)
                //{
                //    ClearNotifications();
                //}
                //else
                //{
                //    await _identityHandler.DeleteAsync(new DeleteUserRequest(request.Id));
                //}
                return;
            };
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
