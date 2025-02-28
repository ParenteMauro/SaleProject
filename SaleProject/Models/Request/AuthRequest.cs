using System.ComponentModel.DataAnnotations;

namespace SaleProject.Models.Request
{
    public class AuthRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
