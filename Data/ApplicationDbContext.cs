using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Memorial> Memorials { get; set; }
        public DbSet<TokenUsage> TokenUsages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            });
            
            // Memorial configuration
            modelBuilder.Entity<Memorial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Biography).HasColumnType("TEXT");
                entity.Property(e => e.BirthDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.DeathDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
                
                // Foreign key relationship
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Memorials)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // TokenUsage configuration
            modelBuilder.Entity<TokenUsage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.LastReset).HasColumnType("timestamp without time zone");
                
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<TokenUsage>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}