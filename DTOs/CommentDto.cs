namespace BlogBackend.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }
    public int? UserId { get; set; }
    public string? DisplayName { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCommentDto
{
    public int BlogPostId { get; set; }
    public int? UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class UpdateCommentDto
{
    public string Content { get; set; } = string.Empty;
}

