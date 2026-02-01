using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Memorial
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public string Name { get; set; } = "";
        
        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public DateTime DeathDate { get; set; }
        
        public string Biography { get; set; } = "";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public User User { get; set; } = null!;
    }
}