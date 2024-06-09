using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
           return await _context.Users.Include(u => u.RoleCode).FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
