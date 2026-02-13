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
        Task<Memorial?> GetMemorialByIdAsync(int id);
        Task<Memorial?> GetMemorialByUniqueIdAsync(string uniqueId);
        Task UpdateMemorialAsync(Memorial memorial);
        Task DeleteMemorialAsync(int id, int userId);
        string GenerateUniqueId(string name);
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
            if (string.IsNullOrEmpty(memorial.UniqueId))
            {
                memorial.UniqueId = GenerateUniqueId(memorial.Name);
            }
            _context.Memorials.Add(memorial);
            await _context.SaveChangesAsync();
            return memorial;
        }
        
        public async Task<Memorial?> GetMemorialByUniqueIdAsync(string uniqueId)
        {
            return await _context.Memorials
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);
        }
        
        public string GenerateUniqueId(string name)
        {
            var slug = name.ToLower()
                .Replace(" ", "-")
                .Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                .Replace("â", "a").Replace("ê", "e").Replace("ô", "o")
                .Replace("ã", "a").Replace("õ", "o")
                .Replace("ç", "c");
            var guid = Guid.NewGuid().ToString().Substring(0, 8);
            return $"{slug}-{guid}";
        }
        
        public async Task<Memorial?> GetMemorialAsync(int id, int userId)
        {
            return await _context.Memorials
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }
        
        public async Task<Memorial?> GetMemorialByIdAsync(int id)
        {
            return await _context.Memorials.FindAsync(id);
        }
        
        public async Task UpdateMemorialAsync(Memorial memorial)
        {
            _context.Memorials.Update(memorial);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteMemorialAsync(int id, int userId)
        {
            var memorial = await GetMemorialAsync(id, userId);
            if (memorial != null)
            {
                _context.Memorials.Remove(memorial);
                await _context.SaveChangesAsync();
            }
        }
    }
}