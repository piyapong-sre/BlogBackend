using BlogBackend.DTOs;
using BlogBackend.Models;
using BlogBackend.Repositories;

namespace BlogBackend.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByBlogPostIdAsync(int blogPostId)
    {
        var comments = await _commentRepository.GetByBlogPostIdAsync(blogPostId);
        return MapToCommentsDto(comments);
    }

    public async Task<CommentDto?> GetCommentByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) return null;

        return MapToCommentDto(comment);
    }

    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createDto)
    {
        var comment = new Comment
        {
            BlogPostId = createDto.BlogPostId,
            UserId = createDto.UserId,
            Content = createDto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _commentRepository.CreateAsync(comment);
        if (created.UserId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(created.UserId.Value);
            created.User = user;
        }
        return MapToCommentDto(created);
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        return await _commentRepository.DeleteAsync(id);
    }

    private IEnumerable<CommentDto> MapToCommentsDto(IEnumerable<Comment> comments)
    {
        var commentDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentDtos.Add(MapToCommentDto(comment));
        }
        return commentDtos;
    }

    private static CommentDto MapToCommentDto(Comment comment)
    {
        var displayName = string.Empty;
        if (comment is { UserId: not null, User: not null })
        {
            displayName = comment.User.DisplayName;
        }

        return new CommentDto
        {
            Id = comment.Id,
            BlogPostId = comment.BlogPostId,
            UserId = comment.UserId,
            DisplayName = displayName,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };
    }
}

