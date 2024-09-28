
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        
        [Required]
        public string? Password { get; set; }
    }
}