using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Services;

public interface ICompanyService
{
    Task Add(Company company);
    Task Update(Company company);
    Task Delete(Guid id);
}
