namespace BlogBackend.Models;

public class Comment
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }
    public int? UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public BlogPost? BlogPost { get; set; }
    public User? User { get; set; }
}
