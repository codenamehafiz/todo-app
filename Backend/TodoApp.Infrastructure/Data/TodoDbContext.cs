using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Infrastructure.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });

            // Seed data
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    Title = "Learn Clean Architecture",
                    Description = "Study the principles of Clean Architecture and implement them in .NET",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Build Todo API",
                    Description = "Create a RESTful API for managing todo items",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            );
        }
    }
}