using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Helpers;
using AssetManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AssetManagementSystem.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<User>> GetUsersAsync(GetUsersRequest request)
        {
            IQueryable<User> query = _context.Users;

            if (request.HideInactive)
            {
                query = query.Where(u => u.IsActive);
            }

            if (!String.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(u => u.FirstName.Contains(request.SearchText) ||
                            u.LastName.Contains(request.SearchText));
            }

            int totalCount = await query.CountAsync();

            List<User> users = await query
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            int totalPages = PaginationHelper.GetTotalPageCount(totalCount, request.PageSize);

            return new PagedResponse<User>
            {
                Items = users,
                Pagination = new PaginationMetadata
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasPreviousPage = request.PageNumber > 1,
                    HasNextPage = request.PageNumber < totalPages
                }
            };
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequest request)
        {
            User user = new()
            {
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user.Id;
        }
        public Task<User?> GetUserByEmailAsync(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        public Task<User?> GetUserByIdAsync(Guid id)
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

        public async Task<bool> UpdateUserRole(Guid id, Role role)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(u => u.Role, role)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> UpdateUserActive(Guid id, bool isActive)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u =>
                    u.SetProperty(u => u.IsActive, isActive)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        
    }
}