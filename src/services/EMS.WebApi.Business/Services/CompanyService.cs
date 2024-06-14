using EMS.Core.Notifications;
using EMS.WebApi.Business.Handlers;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Interfaces.Services;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Business.Utils;

namespace EMS.WebApi.Business.Services;

public class CompanyService : BaseHandler, ICompanyService
{
    public readonly ICompanyRepository _companyRepository;

    public CompanyService(INotifier notifier, IAspNetUser appUser, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _companyRepository = companyRepository;
    }

    public async Task Add(Company company)
    {
        //if (!ExecuteValidation(new CompanyValidation(), company)) return;

        if (CompanyExists(company.Id, company.Document.Number)) return;

        await _companyRepository.AddAsync(company);
        return;
    }

    public Task Update(Company company)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }


    private bool CompanyExists(Guid id, string document)
    {
        if (_companyRepository.SearchAsync(f => f.Document.Number == document && f.Id != id).Result.Any())
        {
            Notify("Este CPF/CNPJ já está em uso.");
            return true;
        }
        return false;
    }
}
