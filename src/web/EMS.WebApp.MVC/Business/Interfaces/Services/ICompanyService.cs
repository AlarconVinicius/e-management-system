using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface ICompanyService
{
    Task<ValidationResult> AddCompany(RegisterCompanyViewModel company);
    //Task<ValidationResult> UpdateCompany(Guid id, UpdateCompanyViewModel usser);
}
