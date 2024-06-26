﻿using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Companies;
using EMS.Core.Requests.Employees;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;
using EMS.Core.Responses.Identities;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;

namespace EMS.WebApi.Business.Handlers;

public class CompanyHandler : BaseHandler, ICompanyHandler
{
    public readonly ICompanyRepository _companyRepository;
    public readonly IEmployeeRepository _employeeRepository;
    public readonly IPlanRepository _planRepository;
    public readonly IEmployeeHandler _employeeHandler;
    public readonly IIdentityHandler _identityHandler;

    public CompanyHandler(INotifier notifier, IAspNetUser appUser, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IPlanRepository planRepository, IEmployeeHandler employeeHandler, IIdentityHandler identityHandler) : base(notifier, appUser)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _planRepository = planRepository;
        _employeeHandler = employeeHandler;
        _identityHandler = identityHandler;
    }

    public async Task<CompanyResponse> GetByIdAsync(GetCompanyByIdRequest request)
    {
        try
        {
            if (!CompanyExists(request.Id)) return null;
            var company = await _companyRepository.GetByIdAsync(request.Id);

            return company.MapCompanyToCompanyResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar a companhia.");
            return null;
        }
    }

    public async Task<PagedResponse<CompanyResponse>> GetAllAsync(GetAllCompaniesRequest request)
    {
        try
        {
            return (await _companyRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, request.Query)).MapPagedCompaniesToPagedResponseCompanies();
        }
        catch
        {
            Notify("Não foi possível consultar as companhias.");
            return null;
        }
    }

    public async Task<LoginUserResponse> CreateAsync(CreateCompanyAndUserRequest request)
    {
        //if (!ExecuteValidation(new CompanyValidation(), company)) return;

        if (!PlanExists(request.Company.PlanId)) return null;
        if (IsDocumentInUse(request.Company.Document)) return null;
        if (IsEmployeeDocumentInUse(request.Employee.Document)) return null;
        var userId = Guid.NewGuid();
        var companyMapped = request.Company.MapCreateCompanyRequestToCompany();
        request.Employee.Id = userId;
        request.Employee.CompanyId = companyMapped.Id;
        request.Employee.Role = Core.Enums.ERoleCore.Admin;
        request.User.Id = userId;
        var employyeeMapped = request.Employee.MapCreateEmployeeRequestToEmployee();
        try
        {
            await _companyRepository.AddAsync(companyMapped);
            await _employeeRepository.AddAsync(employyeeMapped);
            if (!IsOperationValid())
            {
                await DeleteAsync(new DeleteCompanyRequest(companyMapped.Id));
                return null;
            };
            var result = await _identityHandler.CreateAsync(request.User);
            if (!IsOperationValid())
            {
                await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest(request.Employee.Id));
                await DeleteAsync(new DeleteCompanyRequest(companyMapped.Id));
                return null;
            };
            return result;
        }
        catch
        {
            Notify("Não foi possível criar o companhia.");
            return null;
        }
    }

    public async Task CreateAsync(CreateCompanyRequest request)
    {
        //if (!ExecuteValidation(new CompanyValidation(), company)) return;

        if (IsDocumentInUse(request.Document)) return;
        var companyMapped = request.MapCreateCompanyRequestToCompany();
        try
        {
            await _companyRepository.AddAsync(companyMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar a companhia.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateCompanyRequest request)
    {
        //if (!ExecuteValidation(new CompanyValidation(), company)) return;
        if (!CompanyExists(request.Id)) return;
        var companyDb = await _companyRepository.GetByIdAsync(request.Id);

        try
        {
            companyDb.SetName(request.Name);
            companyDb.SetBrand(request.Brand);
            companyDb.SetIsActive(request.IsActive);

            await _companyRepository.UpdateAsync(companyDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar a companhia.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteCompanyRequest request)
    {
        try
        {
            if (!CompanyExists(request.Id)) return;

            await _companyRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar a companhia.");
            return;
        }
    }

    private bool CompanyExists(Guid id)
    {
        if (_companyRepository.SearchAsync(f => f.Id == id).Result.Any())
        {
            return true;
        };

        Notify("Companhia não encontrada.");
        return false;
    }

    private bool IsDocumentInUse(string document)
    {
        if (_companyRepository.SearchAsync(f => f.Document.Number == document).Result.Any())
        {
            Notify("Este CPF/CNPJ já está em uso.");
            return true;
        };
        return false;
    }
    private bool IsEmployeeDocumentInUse(string document)
    {
        if (_employeeRepository.SearchAsync(f => f.Document.Number == document).Result.Any())
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private bool PlanExists(Guid id)
    {
        if (_planRepository.SearchAsync(f => f.Id == id).Result.Any())
        {
            return true;
        };

        Notify("Plano não encontrado.");
        return false;
    }
}
