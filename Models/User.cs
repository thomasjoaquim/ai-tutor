using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        
        [Required]
        public string PasswordHash { get; set; } = "";
        
        [Required]
        public string FirstName { get; set; } = "";
        
        [Required]
        public string LastName { get; set; } = "";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        public bool IsAdmin { get; set; } = false;
        
        public List<Memorial> Memorials { get; set; } = new();
    }
}