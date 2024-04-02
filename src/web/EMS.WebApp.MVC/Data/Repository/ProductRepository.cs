using EMS.WebApp.MVC.Business.Interfaces;
using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Data.Repository;

public class ProductRepository : IProductRepository
{
    private readonly EMSDbContext _context;

    public ProductRepository(EMSDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<PagedResult<Product>> GetAllProducts(int pageSize, int pageIndex, string query = null)
    {
        var productsQuery = _context.Products
            .AsNoTracking();
        if (!string.IsNullOrEmpty(query))
        {
            productsQuery = productsQuery.Where(p => p.Title.Contains(query) || p.Description.Contains(query));
        }
        var products = await productsQuery.OrderBy(p => p.Title)
                                     .ThenByDescending(p => p.UpdatedAt)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize)
                                     .ToListAsync();
        var total = await productsQuery.CountAsync();

        return new PagedResult<Product>()
        {
            List = products,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task<Product> GetById(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        product.UpdateEntityDate();
        _context.Products.Update(product);
    }

    public async Task<bool> DeleteProduct(Product product)
    {
        var productDb = await _context.Products
            .FirstOrDefaultAsync(s => s.Id == product.Id);

        if (product == null!)
        {
            return false;
        }
        _context.Products.Remove(product);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
