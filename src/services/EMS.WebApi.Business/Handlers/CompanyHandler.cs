using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Companies;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;
using EMS.WebApi.Business.Utils;

namespace EMS.WebApi.Business.Handlers;

public class CompanyHandler : BaseHandler, ICompanyHandler
{
    public readonly ICompanyRepository _companyRepository;

    public CompanyHandler(INotifier notifier, IAspNetUser appUser, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _companyRepository = companyRepository;
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
            Notify("Não foi possível criar o companhia.");
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
        if (TenantIsEmpty()) return;
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
}
