using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.Data.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(EMSDbContext context) : base (context)
    {
    }

    public async Task<PagedResult<Company>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null)
    {
        var companiesQuery = DbSet.AsNoTracking();
        if (!string.IsNullOrEmpty(query))
        {
            companiesQuery = companiesQuery.Where(p => p.Name.Contains(query) || p.Document.Number.Contains(query));
        }
        var users = await companiesQuery.OrderBy(p => p.Name)
                                        .ThenByDescending(p => p.UpdatedAt)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize)
                                        .ToListAsync();
        var total = await companiesQuery.CountAsync();

        return new PagedResult<Company>()
        {
            List = users,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task Block(Guid id)
    {
        var companyDb = await GetByIdAsync(id);
        if (companyDb != null)
        {
            if (companyDb.IsActive)
            {
                companyDb.BlockCompany(false);
            }
            else
            {
                companyDb.BlockCompany(true);
            }
            Db.Companies.Update(companyDb);
            return;
        }
    }

}