using BlogBackend.DTOs;

namespace BlogBackend.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsByBlogPostIdAsync(int blogPostId);
    Task<CommentDto?> GetCommentByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(CreateCommentDto createDto);
    Task<bool> DeleteCommentAsync(int id);
}

