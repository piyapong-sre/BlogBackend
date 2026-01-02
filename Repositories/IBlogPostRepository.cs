using BlogBackend.Models;

namespace BlogBackend.Repositories;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<IEnumerable<BlogPost>> GetPublishedAsync();
    Task<BlogPost?> GetByIdAsync(int id);
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<BlogPost?> UpdateAsync(int id, BlogPost blogPost);
    Task<bool> DeleteAsync(int id);
}
