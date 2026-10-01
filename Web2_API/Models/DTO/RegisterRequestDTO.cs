using System.ComponentModel.DataAnnotations;

namespace Web2_API.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string[]? Roles { get; set; }
    }
}