using Repository.Models;

namespace Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> CreateUser(User user);
        Task<bool> UpdateRefreshToken(Guid userId, string refreshToken);
        Task<User> GetRefreshToken(string refreshToken);

        Task<bool> DeleteRefreshToken(Guid userId);
        Task<bool> SaveChanges();
    }
}
