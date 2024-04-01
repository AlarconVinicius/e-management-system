using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface ICompanyService
{
    Task<ValidationResult> AddCompany(CompanyViewModel company);
    //Task<ValidationResult> UpdateCompany(Guid id, UpdateCompanyViewModel usser);
}
