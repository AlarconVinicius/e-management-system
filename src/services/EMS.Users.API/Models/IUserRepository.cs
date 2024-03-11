using EMS.Core.Data;

namespace EMS.Users.API.Models;

public interface IUserRepository : IRepository<User>
{
    void Add(User user);
    void Update(User user);
    void Delete(User user);

    Task<IEnumerable<User>> GetAll();
    Task<User> GetByCpf(string cpf);
    Task<User> GetById(Guid id);

    void AddAddress(Address address);
    Task<Address> GetAddressById(Guid id);
}
