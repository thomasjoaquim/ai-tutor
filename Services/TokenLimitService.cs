using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Services
{
    public class TokenLimitService
    {
        private readonly ApplicationDbContext _context;
        private const int MONTHLY_TOKEN_LIMIT = 10000;

        public TokenLimitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanUseTokensAsync(int userId, int tokensNeeded)
        {
            var usage = await GetOrCreateTokenUsageAsync(userId);
            
            // Reset if it's a new month
            if (usage.LastReset.Month != DateTime.UtcNow.Month || usage.LastReset.Year != DateTime.UtcNow.Year)
            {
                usage.TokensUsed = 0;
                usage.LastReset = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            
            return usage.TokensUsed + tokensNeeded <= MONTHLY_TOKEN_LIMIT;
        }

        public async Task AddTokenUsageAsync(int userId, int tokensUsed)
        {
            var usage = await GetOrCreateTokenUsageAsync(userId);
            usage.TokensUsed += tokensUsed;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetRemainingTokensAsync(int userId)
        {
            var usage = await GetOrCreateTokenUsageAsync(userId);
            
            // Reset if it's a new month
            if (usage.LastReset.Month != DateTime.UtcNow.Month || usage.LastReset.Year != DateTime.UtcNow.Year)
            {
                usage.TokensUsed = 0;
                usage.LastReset = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            
            return Math.Max(0, MONTHLY_TOKEN_LIMIT - usage.TokensUsed);
        }

        private async Task<TokenUsage> GetOrCreateTokenUsageAsync(int userId)
        {
            var usage = await _context.TokenUsages.FirstOrDefaultAsync(t => t.UserId == userId);
            
            if (usage == null)
            {
                usage = new TokenUsage { UserId = userId };
                _context.TokenUsages.Add(usage);
                await _context.SaveChangesAsync();
            }
            
            return usage;
        }
    }
}