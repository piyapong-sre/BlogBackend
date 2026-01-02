using Microsoft.EntityFrameworkCore;
using BlogBackend.Data;
using BlogBackend.Models;

namespace BlogBackend.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly ApplicationDbContext _context;

    public BlogPostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(bp => bp.CreatedAt)
            .Select(c=> new BlogPost
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                ContentImage =  c.ContentImage,
                UserId = c.UserId,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                User = c.User == null
                    ? null
                    : new User
                    {
                        Id = c.User.Id,
                        DisplayName = c.User.DisplayName
                    }
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetPublishedAsync()
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .Where(bp => bp.IsPublished)
            .OrderByDescending(bp => bp.CreatedAt)
            .Select(c=> new BlogPost
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                ContentImage =  c.ContentImage,
                UserId = c.UserId,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                User = c.User == null
                    ? null
                    : new User
                    {
                        Id = c.User.Id,
                        DisplayName = c.User.DisplayName
                    }
            })
            .ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(int id)
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .Select(c=> new BlogPost
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                ContentImage =  c.ContentImage,
                UserId = c.UserId,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                User = c.User == null
                    ? null
                    : new User
                    {
                        Id = c.User.Id,
                        DisplayName = c.User.DisplayName
                    }
            })
            .FirstOrDefaultAsync(bp => bp.Id == id);
    }

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();

       return await GetByIdAsync(blogPost.Id) ?? throw new InvalidOperationException();
    }

    public async Task<BlogPost?> UpdateAsync(int id, BlogPost blogPost)
    {
        var existingPost = await _context.BlogPosts.FindAsync(id);
        if (existingPost == null)
            return null;

        existingPost.Title = blogPost.Title;
        existingPost.Content = blogPost.Content;
        existingPost.ContentImage = blogPost.ContentImage;
        existingPost.UserId = blogPost.UserId;
        existingPost.IsPublished = blogPost.IsPublished;
        existingPost.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(existingPost.Id) ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null)
            return false;

        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();
        return true;
    }
}
