using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Utils.User;
using EMS.WebApp.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public class UserRepository : IUserRepository
{
    private readonly EMSDbContext _context;
    private readonly IAspNetUser _aspNetUser;

    public UserRepository(EMSDbContext context, IAspNetUser aspNetUser)
    {
        _context = context;
        _aspNetUser = aspNetUser;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<PagedResult<User>> GetAllUsers(int pageSize, int pageIndex, string query = null)
    {
        var tenantId = _aspNetUser.GetTenantId();
        if (tenantId == Guid.Empty)
        {
            return null;
        }
        var usersQuery = _context.Users
            .AsNoTracking();
        if (!string.IsNullOrEmpty(query))
        {
            usersQuery = usersQuery.Where(p => p.Name.Contains(query) || p.LastName.Contains(query) || p.Email.Address.Contains(query));
        }
        var users = await usersQuery.Where(p => p.TenantId == tenantId)
                                     .OrderBy(p => p.Name)
                                     .ThenByDescending(p => p.UpdatedAt)
                                     .Skip(pageSize * (pageIndex - 1))
                                     .Take(pageSize)
                                     .ToListAsync();
        var total = await usersQuery.CountAsync();

        return new PagedResult<User>()
        {
            List = users,
            TotalResults = total,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Query = query
        };
    }

    public async Task<User> GetById(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<User> GetByCpf(string cpf)
    {
        return await _context.Users.FirstOrDefaultAsync(c => c.Cpf.Number == cpf) ?? null!;
    }

    public void AddUser(User subscriber)
    {
        _context.Users.Add(subscriber);
    }

    public void UpdateUser(User subscriber)
    {
        _context.Users.Update(subscriber);
    }

    public async Task<bool> DeleteUser(User subscriber)
    {
        var subscriberDb = await _context.Users
            //.Include(s => s.Workers)
            //.Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == subscriber.Id);

        if (subscriber == null!)
        {
            return false;
        }
        _context.Users.Remove(subscriber);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}