using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Business.Utils;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.Data.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly IAspNetUser _aspNetUser;
    private readonly Guid _tenantId;

    public ProductRepository(EMSDbContext context, IAspNetUser aspNetUser) : base (context)
    {
        _aspNetUser = aspNetUser;

        _tenantId = _aspNetUser.GetTenantId() != Guid.Empty ? _aspNetUser.GetTenantId() : Guid.Empty;
    }

    public override async Task<IEnumerable<Product>> SearchAsync(Expression<Func<Product, bool>> predicate)
    {
        return await DbSet.AsNoTracking()
                                .Where(p => p.CompanyId == _tenantId)
                                .Where(predicate)
                                .ToListAsync();
    }

    public async override Task<Product> GetByIdAsync(Guid id)
    {
        return await DbSet.Where(p => p.CompanyId == _tenantId)
                          .FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<PagedResult<Product>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null)
    {
        var productsQuery = DbSet.Where(p => p.CompanyId == _tenantId).AsNoTracking();

        if (!string.IsNullOrEmpty(query))
        {
            productsQuery = productsQuery.Where(p => p.Title.Contains(query));
        }
        var users = await productsQuery.OrderBy(p => p.Title)
                                       .ThenByDescending(p => p.UpdatedAt)
                                       .Skip(pageSize * (pageIndex - 1))
                                       .Take(pageSize)
                                       .ToListAsync();
        var total = await productsQuery.CountAsync();

        return new PagedResult<Product>()
        {
            List = users,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

}