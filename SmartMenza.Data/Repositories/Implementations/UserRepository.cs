using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Data;
using SmartMenza.Data.Entities;
using SmartMenza.Data.Repositories.Interfaces;

namespace SmartMenza.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllAsync()
            => _context.Users.ToListAsync();

        public Task<User?> GetByEmailAndPasswordAsync(string email, string password)
            => _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

        public Task<User?> GetByEmailAsync(string email)
            => _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
