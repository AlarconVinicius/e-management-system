using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface IUserRepository : IRepository<User>
{
    void AddUser(User subscriber);
    void UpdateUser(User subscriber);
    Task<bool> DeleteUser(User subscriber);
    Task<PagedResult<User>> GetAllUsers(int pageSize, int pageIndex, string query = null);
    Task<User> GetById(Guid id);
    Task<User> GetByCpf(string cpf);
}
