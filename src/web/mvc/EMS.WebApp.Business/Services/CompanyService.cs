using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Interfaces.Services;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Notifications;

namespace EMS.WebApp.Business.Services;

public class CompanyService : MainService, ICompanyService
{
    public readonly ICompanyRepository _companyRepository;

    public CompanyService(INotifier notifier, ICompanyRepository companyRepository) : base(notifier)
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
