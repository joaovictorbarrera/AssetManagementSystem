using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Data;
using ThreatlockerAssetManagementSystem.Models.Entities;

namespace ThreatlockerAssetManagementSystem.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        public Task<User?> GetById(Guid id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateLastLoginAsync(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u =>
                    u.SetProperty(x => x.LastLoginAt, DateTime.UtcNow)
                );
        }
    }
}