using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Services
{
    public interface IMemorialService
    {
        Task<List<Memorial>> GetUserMemorialsAsync(int userId);
        Task<Memorial> CreateMemorialAsync(Memorial memorial);
        Task<Memorial?> GetMemorialAsync(int id, int userId);
    }
    
    public class MemorialService : IMemorialService
    {
        private readonly ApplicationDbContext _context;
        
        public MemorialService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Memorial>> GetUserMemorialsAsync(int userId)
        {
            return await _context.Memorials
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<Memorial> CreateMemorialAsync(Memorial memorial)
        {
            memorial.CreatedAt = DateTime.UtcNow;
            _context.Memorials.Add(memorial);
            await _context.SaveChangesAsync();
            return memorial;
        }
        
        public async Task<Memorial?> GetMemorialAsync(int id, int userId)
        {
            return await _context.Memorials
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }
    }
}