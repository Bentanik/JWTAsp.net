using Repository.Interfaces;
using Repository.Models;
using Repository.ViewModels.Responses;
using Service.Interfaces;

namespace Service.Repositories
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
        }
    }
}
