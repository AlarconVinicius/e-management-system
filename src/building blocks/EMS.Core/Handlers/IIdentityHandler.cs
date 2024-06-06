using EMS.Core.Requests.Identities;
using EMS.Core.Responses;
using EMS.Core.Responses.Identities;

namespace EMS.Core.Handlers;

public interface IIdentityHandler
{
    Task<LoginUserResponse> LoginAsync(LoginUserRequest request);
    Task Logout();
    Task<LoginUserResponse> CreateAsync(CreateUserRequest request);
    Task UpdateEmailAsync(UpdateUserEmailRequest request);
    Task UpdatePasswordAsync(UpdateUserPasswordRequest request);
    Task AddOrUpdateUserClaimAsync(AddOrUpdateUserClaimRequest request);
    Task DeleteAsync(DeleteUserRequest request);
    Task<UserResponse> GetByIdAsync(GetUserByIdRequest request);
    Task<PagedResponse<UserResponse>> GetAllAsync(GetAllUsersRequest request);
}