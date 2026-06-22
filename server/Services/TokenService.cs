using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            string jwtKey = _configuration["JwtKey"]
                ?? throw new Exception("JwtKey missing from configuration.");

            int expirationDays =_configuration.GetValue("TokenExpirationDays", 7);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expirationDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid? GetUserIdFromToken(string token)
        {
            string jwtKey = _configuration["JwtKey"]
                ?? throw new Exception("JwtKey missing from configuration.");

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey))
                    },
                    out _);

                string? userId = principal.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                return userId == null
                    ? null
                    : Guid.Parse(userId);
            }
            catch
            {
                return null;
            }
        }
    }
}
