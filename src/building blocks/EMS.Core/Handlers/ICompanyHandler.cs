using EMS.Core.Requests.Companies;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;

namespace EMS.Core.Handlers;

public interface ICompanyHandler
{
    Task CreateAsync(CreateCompanyRequest request);
    Task UpdateAsync(UpdateCompanyRequest request);
    Task DeleteAsync(DeleteCompanyRequest request);
    Task<CompanyResponse> GetByIdAsync(GetCompanyByIdRequest request);
    Task<PagedResponse<CompanyResponse>> GetAllAsync(GetAllCompaniesRequest request);
}