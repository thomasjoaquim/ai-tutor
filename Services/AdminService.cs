using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Services
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<Memorial>> GetAllMemorialsAsync();
        Task<List<TokenUsage>> GetAllTokenUsageAsync();
        Task<AdminDashboardStats> GetDashboardStatsAsync();
    }

    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Memorials)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Memorial>> GetAllMemorialsAsync()
        {
            return await _context.Memorials
                .Include(m => m.User)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TokenUsage>> GetAllTokenUsageAsync()
        {
            return await _context.TokenUsages
                .Include(t => t.User)
                .OrderByDescending(t => t.TokensUsed)
                .ToListAsync();
        }

        public async Task<AdminDashboardStats> GetDashboardStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalMemorials = await _context.Memorials.CountAsync();
            var totalTokensUsed = await _context.TokenUsages.SumAsync(t => t.TokensUsed);
            var activeUsersThisMonth = await _context.TokenUsages
                .Where(t => t.LastReset.Month == DateTime.UtcNow.Month && t.LastReset.Year == DateTime.UtcNow.Year)
                .CountAsync();

            return new AdminDashboardStats
            {
                TotalUsers = totalUsers,
                TotalMemorials = totalMemorials,
                TotalTokensUsed = totalTokensUsed,
                ActiveUsersThisMonth = activeUsersThisMonth
            };
        }
    }

    public class AdminDashboardStats
    {
        public int TotalUsers { get; set; }
        public int TotalMemorials { get; set; }
        public int TotalTokensUsed { get; set; }
        public int ActiveUsersThisMonth { get; set; }
    }
}