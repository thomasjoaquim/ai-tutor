using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class TokenUsage
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int TokensUsed { get; set; } = 0;
        
        public DateTime LastReset { get; set; } = DateTime.UtcNow;
        
        public User User { get; set; } = null!;
    }
}