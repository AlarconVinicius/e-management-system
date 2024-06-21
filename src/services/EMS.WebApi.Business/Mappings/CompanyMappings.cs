using EMS.Core.Requests.Companies;
using EMS.Core.Responses;
using EMS.Core.Responses.Companies;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class CompanyMappings
{
    public static CompanyResponse MapCompanyToCompanyResponse(this Company company)
    {
        if (company == null)
        {
            return null;
        }

        return new CompanyResponse(company.Id, company.PlanId, company.Name, company.Brand, company.Document.Number, company.IsActive, company.CreatedAt, company.UpdatedAt);
    }

    public static Company MapCompanyResponseToCompany(this CompanyResponse companyResponse)
    {
        if (companyResponse == null)
        {
            return null;
        }

        return new Company(companyResponse.Id, companyResponse.PlanId, companyResponse.Name, companyResponse.Brand, companyResponse.Document, companyResponse.IsActive);
    }

    public static PagedResponse<CompanyResponse> MapPagedCompaniesToPagedResponseCompanies(this PagedResult<Company> company)
    {
        if (company == null)
        {
            return null;
        }

        return new PagedResponse<CompanyResponse>(company.List.Select(x => x.MapCompanyToCompanyResponse()).ToList(), company.TotalResults, company.PageIndex, company.PageSize);
    }

    public static Company MapCreateCompanyRequestToCompany(this CreateCompanyRequest companyRequest)
    {
        if (companyRequest == null)
        {
            return null;
        }

        return new Company(companyRequest.PlanId, companyRequest.Name, companyRequest.Brand, companyRequest.Document, companyRequest.IsActive);
    }
}