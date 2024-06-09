using Repository.ViewModels;
using Repository.ViewModels.Responses;
using Repository.Models;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse<object>> CreateUser(UserDto userDto);
        Task<AuthenticationUserResponse> LoginUser(User user);
        Task<AuthenticationUserResponse> RefreshToken(string token);
        Task<bool> DeleteRefreshToken(Guid userId);
    }
}
