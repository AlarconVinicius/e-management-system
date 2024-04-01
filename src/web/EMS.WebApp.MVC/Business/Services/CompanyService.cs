﻿using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces.Services;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Services.Notifications;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Services;

public class CompanyService : MainService, ICompanyService
{
    public readonly ICompanyRepository _companyRepository;

    public CompanyService(INotifier notifier, ICompanyRepository companyRepository) : base(notifier)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ValidationResult> AddCompany(RegisterCompanyViewModel company)
    {
        if (await CompanyExists(company.Cpf)) return _validationResult;
        _companyRepository.AddCompany(new Company(company.PlanId, company.CompanyName, company.CpfOrCnpj, true));
        return _validationResult;
    }

    private async Task<bool> CompanyExists(string cpf)
    {
        var userExist = await _companyRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF/CNPJ já está em uso.");
            return true;
        };
        return false;
    }
}
