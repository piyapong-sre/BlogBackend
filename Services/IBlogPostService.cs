using BlogBackend.DTOs;

namespace BlogBackend.Services;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPostDto>> GetAllPostsAsync();
    Task<IEnumerable<BlogPostDto>> GetPublishedPostsAsync();
    Task<BlogPostDto?> GetPostByIdAsync(int id);
    Task<BlogPostDto> CreatePostAsync(CreateBlogPostDto createDto);
    Task<BlogPostDto?> UpdatePostAsync(int id, UpdateBlogPostDto updateDto);
    Task<bool> DeletePostAsync(int id);
}
