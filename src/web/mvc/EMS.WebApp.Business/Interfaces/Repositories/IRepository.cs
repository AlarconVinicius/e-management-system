using EMS.WebApp.Business.Models;
using System.Linq.Expressions;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface IRepository<T> : IDisposable where T : Entity
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<PagedResult<T>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
    Task<int> SaveChangesAsync();
}