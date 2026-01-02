using BlogBackend.DTOs;
using BlogBackend.Models;
using BlogBackend.Repositories;

namespace BlogBackend.Services;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _repository;

    public BlogPostService(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BlogPostDto>> GetAllPostsAsync()
    {
        var posts = await _repository.GetAllAsync();
        return posts.Select(MapToDto);
    }

    public async Task<IEnumerable<BlogPostDto>> GetPublishedPostsAsync()
    {
        var posts = await _repository.GetPublishedAsync();
        return posts.Select(MapToDto);
    }

    public async Task<BlogPostDto?> GetPostByIdAsync(int id)
    {
        var post = await _repository.GetByIdAsync(id);
        return post == null ? null : MapToDto(post);
    }

    public async Task<BlogPostDto> CreatePostAsync(CreateBlogPostDto createDto)
    {
        var blogPost = new BlogPost
        {
            Title = createDto.Title,
            Content = createDto.Content,
            UserId = createDto.UserId,
            IsPublished = createDto.IsPublished,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(blogPost);
        return MapToDto(created);
    }

    public async Task<BlogPostDto?> UpdatePostAsync(int id, UpdateBlogPostDto updateDto)
    {
        var existingPost = await _repository.GetByIdAsync(id);
        if (existingPost == null)
            return null;

        existingPost.Title = updateDto.Title ?? existingPost.Title;
        existingPost.Content = updateDto.Content ?? existingPost.Content;
        existingPost.ContentImage = updateDto.ContentImage ?? existingPost.ContentImage;
        existingPost.IsPublished = updateDto.IsPublished ?? existingPost.IsPublished;

        var updated = await _repository.UpdateAsync(id, existingPost);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static BlogPostDto MapToDto(BlogPost post)
    {
        var displayName = post is { UserId: not null, User: not null }
            ? post.User.DisplayName
            : "Unknown";
        return new BlogPostDto(
            post.Id,
            post.Title,
            post.Content,
            post.ContentImage,
            post.UserId ?? 0,
            displayName,
            post.CreatedAt,
            post.UpdatedAt,
            post.IsPublished
        );
    }
}
