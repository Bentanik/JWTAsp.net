using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interfaces;
using Repository.Models;
using System.ComponentModel;


namespace Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            // Get role user(RoleId = 2)
            var roleUser = await _context.RoleCodes.FirstOrDefaultAsync(r => r.Id == 2);
            user.RoleCode = roleUser;
            await _context.AddAsync(user);
            return await SaveChanges();
        }

        public async Task<bool> UpdateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.RefreshToken = refreshToken;
            return await SaveChanges();
        }

        public async Task<User> GetRefreshToken(string refreshToken)
        {
            return await _context.Users.Include(u => u.RoleCode).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> DeleteRefreshToken(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Id == userId);
            user.RefreshToken = "";
            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 && true;
        }
       
    }
}
