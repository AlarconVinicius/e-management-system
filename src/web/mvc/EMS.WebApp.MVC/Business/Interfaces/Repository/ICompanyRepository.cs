using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface ICompanyRepository : IRepository<Company>
{
    void AddCompany(Company company);
    void UpdateCompany(Company company);
    Task<bool> DeleteCompany(Company company);
    Task<IEnumerable<Company>> GetAllCompanies();
    Task<Company> GetById(Guid id);
    Task<Company> GetByCpf(string cpf);
}
