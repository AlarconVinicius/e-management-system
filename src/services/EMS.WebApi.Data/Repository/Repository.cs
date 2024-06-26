using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.Data.Repository;
public class Repository<T> : IRepository<T> where T : Entity, new()
{
    protected readonly EMSDbContext Db;
    protected readonly DbSet<T> DbSet;

    protected Repository(EMSDbContext db)
    {
        Db = db;
        DbSet = db.Set<T>();
    }
    public virtual async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        entity.SetCreatedAtDate();
        entity.SetUpdatedAtDate();
        DbSet.Add(entity);
        await SaveChangesAsync();
    }
    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.SetCreatedAtDate();
            entity.SetUpdatedAtDate();
        }

        DbSet.AddRange(entities);
        await SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        entity.SetUpdatedAtDate();
        DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        DbSet.Remove(new T { Id = id });
        await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}
