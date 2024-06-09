using Repository.Models;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
    }
}
