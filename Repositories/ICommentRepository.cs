using BlogBackend.Models;

namespace BlogBackend.Repositories;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByBlogPostIdAsync(int blogPostId);
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment comment);
    Task<bool> DeleteAsync(int id);
}

