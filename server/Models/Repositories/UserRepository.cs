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

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
        }
    }
}