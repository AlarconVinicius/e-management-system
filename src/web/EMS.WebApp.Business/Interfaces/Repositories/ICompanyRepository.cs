using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<bool> Block(Guid id);
}
