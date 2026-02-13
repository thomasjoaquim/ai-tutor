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
                
                // Check if UniqueId column exists, if not add it
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();
                
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Memorials' 
                    AND COLUMN_NAME = 'UniqueId'";
                
                var columnExists = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
                
                if (!columnExists)
                {
                    command.CommandText = "ALTER TABLE Memorials ADD COLUMN UniqueId VARCHAR(255) NOT NULL DEFAULT ''";
                    await command.ExecuteNonQueryAsync();
                    
                    command.CommandText = "CREATE INDEX IX_Memorials_UniqueId ON Memorials(UniqueId)";
                    await command.ExecuteNonQueryAsync();
                    
                    Console.WriteLine("UniqueId column added successfully");
                }
                
                await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}