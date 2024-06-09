using EMS.WebApp.Identity.Business.Models;

namespace EMS.WebApp.Identity.Business.Interfaces.Services;

public interface IAuthService
{
    Task<User> GetUserById(string userId);
    Task Login(LoginUser loginUser);
    Task Logout();
    Task RegisterUser(RegisterUser registerUser);
    Task UpdateUserEmail(string userId, string newEmail);
    Task UpdatePassword(UpdateUserPassword updatePassword);
    Task DeleteUser(string userId);
    Task AddOrUpdateUserClaim(string userId, string type, string value);
}
