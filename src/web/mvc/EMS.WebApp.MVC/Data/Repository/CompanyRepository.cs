using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly EMSDbContext _context;

    public CompanyRepository(EMSDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Company>> GetAllCompanies()
    {
        return await _context.Companies.AsNoTracking().ToListAsync();
    }

    public async Task<Company> GetById(Guid id)
    {
        return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<Company> GetByCpf(string cpf)
    {
        return await _context.Companies.FirstOrDefaultAsync(c => c.CpfOrCnpj.Number == cpf) ?? null!;
    }

    public void AddCompany(Company subscriber)
    {
        _context.Companies.Add(subscriber);
    }

    public void UpdateCompany(Company subscriber)
    {
        _context.Companies.Update(subscriber);
    }

    public async Task<bool> DeleteCompany(Company subscriber)
    {
        var subscriberDb = await _context.Companies
            //.Include(s => s.Workers)
            //.Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == subscriber.Id);

        if (subscriber == null!)
        {
            return false;
        }
        _context.Companies.Remove(subscriber);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}