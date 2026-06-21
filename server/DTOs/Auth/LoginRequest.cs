using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.Auth
{
    public class LoginRequest
    {
        [EmailAddress]
        public required string EmailAddress { get; set; }
    }
}
