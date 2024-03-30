using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public class UserRepository : IUserRepository
{
    private readonly EMSDbContext _context;

    public UserRepository(EMSDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
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