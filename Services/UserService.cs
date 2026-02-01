using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;
using BCrypt.Net;

namespace MyProject.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User> RegisterAsync(RegisterModel model);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
    }
    
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
                
            return user;
        }
        
        public async Task<User> RegisterAsync(RegisterModel model)
        {
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }
    }
}