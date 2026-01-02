using BlogBackend.Models;

namespace BlogBackend.Data;

public class DbSeeder(ApplicationDbContext context)
{
    public async Task SeedAsync()
    {
   
        if (context.Users.Any() || context.BlogPosts.Any())
        {
            Console.WriteLine("Database already seeded. Skipping seed data.");
            return;
        }

        Console.WriteLine("Starting database seeding...");


        var users = new[]
        {
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@blog.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                DisplayName = "Admin User",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new User
            {
                Id = 2,
                Username = "user01",
                Email = "change@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                DisplayName = "Change Can",
                CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new User
            {
                Id = 3,
                Username = "user02",
                Email = "blend@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                DisplayName = "Blend 285",
                CreatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
        Console.WriteLine($"Seeded {users.Length} users.");
        
        var blogPosts = new[]
        {
            new BlogPost
            {
                Id = 1,
                Title = "IT 08-1",
                Content = "",
                ContentImage = "https://images.unsplash.com/photo-1548199973-03cce0bbc87b?w=800",
                UserId = 2,
                CreatedAt = new DateTime(2024, 1, 5, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 5, 10, 0, 0, DateTimeKind.Utc),
                IsPublished = true,
            },
        };

        await context.BlogPosts.AddRangeAsync(blogPosts);
        await context.SaveChangesAsync();
        Console.WriteLine($"Seeded {blogPosts.Length} blog posts.");

        Console.WriteLine("Database seeding completed successfully!");
    }
}

