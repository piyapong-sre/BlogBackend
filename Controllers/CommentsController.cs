using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BlogBackend.DTOs;
using BlogBackend.Services;
using BlogBackend.Hubs;
using Microsoft.AspNetCore.Authorization;
using BlogBackend.Extensions;

namespace BlogBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;
    private readonly IHubContext<CommentHub> _hubContext;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(
        ICommentService service,
        IHubContext<CommentHub> hubContext,
        ILogger<CommentsController> logger)
    {
        _service = service;
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpGet("blogpost/{blogPostId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByBlogPostId(int blogPostId)
    {
        var comments = await _service.GetCommentsByBlogPostIdAsync(blogPostId);
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetById(int id)
    {
        var comment = await _service.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }
        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserIdOrNull();
        if (userId == null)
        {
            return Unauthorized(new { message = "Invalid user token" });
        }

        createDto.UserId = userId.Value;

        var created = await _service.CreateCommentAsync(createDto);
        
        await _hubContext.Clients.All.SendAsync("ReceiveComment", created);
        
        await _hubContext.Clients.Group($"BlogPost_{created.BlogPostId}")
            .SendAsync("ReceiveComment", created);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var comment = await _service.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(new { message = $"Comment with ID {id} not found" });
        }

        var deleted = await _service.DeleteCommentAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Failed to delete comment with ID {id}" });
        }
        
        await _hubContext.Clients.All.SendAsync("CommentDeleted", id);
        await _hubContext.Clients.Group($"BlogPost_{comment.BlogPostId}")
            .SendAsync("CommentDeleted", id);

        return NoContent();
    }
}

