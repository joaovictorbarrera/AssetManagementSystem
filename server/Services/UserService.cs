using AssetManagementSystem.DTOs.Auth;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly TokenService _tokenService;
        public UserService(UserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<PagedResponse<User>>> Get([FromQuery] GetUsersRequest request)
        {
            PagedResponse<User> users = await _userRepository.GetUsersAsync(request);

            return ServiceResult<PagedResponse<User>>.Success(users);
        }

        public async Task<ServiceResult<Guid>> Create([FromBody] CreateUserRequest request)
        {
            bool userExists = await _userRepository.GetUserByEmailAsync(request.EmailAddress) != null;
            if (userExists) return ServiceResult<Guid>.BadRequest("Email Address is taken");

            Guid newUserId = await _userRepository.CreateUserAsync(request);

            return ServiceResult<Guid>.Success(newUserId);
        }

        public async Task<ServiceResult> UpdateRole(Guid id, [FromBody] UpdateUserRoleRequest request)
        {
            bool success = await _userRepository.UpdateUserRole(id, request.Role);

            return success ? ServiceResult.Success() : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> UpdateActive(Guid id, UpdateUserActiveRequest request)
        {
            bool success = await _userRepository.UpdateUserActive(id, request.IsActive);

            return success ? ServiceResult.Success() : ServiceResult.NotFound();
        }

        public async Task<ServiceResult<TokenDto>> Login([FromBody] LoginRequest loginData)
        {
            // There is intentionally no check for password.
            // Managing passwords is outside the scope of this project.
            User? user = await _userRepository.GetUserByEmailAsync(loginData.EmailAddress);

            if (user == null || !user.IsActive)
            {
                return ServiceResult<TokenDto>.Unauthorized();
            }

            TokenDto tokenDto = _tokenService.CreateToken(user);

            await _userRepository.UpdateLastLoginAsync(user.Id);

            return ServiceResult<TokenDto>.Success(tokenDto);
        }
    }
}
