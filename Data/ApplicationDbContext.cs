using Microsoft.EntityFrameworkCore;
using BlogBackend.Models;

namespace BlogBackend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsPublished);

            entity.HasOne(e => e.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();

            entity.HasIndex(e => e.BlogPostId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(e => e.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasMany(e => e.BlogPosts)
                .WithOne(bp => bp.User)
                .HasForeignKey(bp => bp.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

    }
}
