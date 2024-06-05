using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Services;

public interface ICompanyService
{
    Task Add(Company company);
    Task Update(Company company);
    Task Delete(Guid id);
}
