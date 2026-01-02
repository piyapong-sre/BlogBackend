namespace BlogBackend.DTOs;

public record BlogPostDto(
    int Id,
    string Title,
    string Content,
    string ContentImage,
    int UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsPublished
);

public record CreateBlogPostDto(
    string Title,
    string Content,
    string ContentImage,
    bool IsPublished = false
)
{
    public int UserId { get; init; }
};

public record UpdateBlogPostDto(
    string? Title,
    string? Content,
    string? ContentImage,
    bool? IsPublished
);
