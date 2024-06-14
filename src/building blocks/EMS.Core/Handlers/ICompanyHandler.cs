using EMS.Core.Requests.Companies;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;
using EMS.Core.Responses.Identities;

namespace EMS.Core.Handlers;

public interface ICompanyHandler
{
    Task CreateAsync(CreateCompanyRequest request);
    Task<LoginUserResponse> CreateAsync(CreateCompanyAndUserRequest request);
    Task UpdateAsync(UpdateCompanyRequest request);
    Task DeleteAsync(DeleteCompanyRequest request);
    Task<CompanyResponse> GetByIdAsync(GetCompanyByIdRequest request);
    Task<PagedResponse<CompanyResponse>> GetAllAsync(GetAllCompaniesRequest request);
}