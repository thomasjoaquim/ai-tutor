using Microsoft.EntityFrameworkCore;
using MyProject.Data;

namespace MyProject.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            try
            {
                await context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}