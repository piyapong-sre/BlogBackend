using Microsoft.EntityFrameworkCore;
using BlogBackend.Data;
using BlogBackend.Models;

namespace BlogBackend.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetByBlogPostIdAsync(int blogPostId)
    {
        return await _context.Comments
            .AsNoTracking()
            .Where(c => c.BlogPostId == blogPostId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new Comment
            {
                Id = c.Id,
                BlogPostId = c.BlogPostId,
                UserId = c.UserId,
                Content = c.Content,
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

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments
            .Include(c => c.BlogPost)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return false;

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
}

