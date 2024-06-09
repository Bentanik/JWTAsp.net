using Repository.Models;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
    }
}
