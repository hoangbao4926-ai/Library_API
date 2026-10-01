using System.ComponentModel.DataAnnotations;

namespace Web2_API.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}