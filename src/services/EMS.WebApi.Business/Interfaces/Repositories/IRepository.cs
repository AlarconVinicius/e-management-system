using EMS.WebApi.Business.Models;
using System.Linq.Expressions;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IRepository<T> : IDisposable where T : Entity
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
    Task<int> SaveChangesAsync();
}