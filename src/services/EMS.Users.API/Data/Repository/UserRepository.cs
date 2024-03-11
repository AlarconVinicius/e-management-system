using EMS.Core.Data;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Users.API.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly UsersContext _context;

    public UserRepository(UsersContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public Task<User> GetByCpf(string cpf)
    {
        return _context.Users.FirstOrDefaultAsync(c => c.Cpf.Number == cpf)!;
    }

    public Task<User> GetById(Guid id)
    {
        return _context.Users.FirstOrDefaultAsync(c => c.Id == id)!;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<Address> GetAddressById(Guid id)
    {
        return await _context.Addresses.FirstOrDefaultAsync(e => e.UserId == id) ?? null!;
    }

    public void AddAddress(Address address)
    {
        _context.Addresses.Add(address);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
