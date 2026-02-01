using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        
        public bool RememberMe { get; set; }
    }
    
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
        
        [Required]
        public string FirstName { get; set; } = "";
        
        [Required]
        public string LastName { get; set; } = "";
    }
}