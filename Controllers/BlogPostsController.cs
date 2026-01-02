using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogBackend.DTOs;
using BlogBackend.Services;
using BlogBackend.Extensions;

namespace BlogBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostService _service;
    private readonly ILogger<BlogPostsController> _logger;

    public BlogPostsController(IBlogPostService service, ILogger<BlogPostsController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetAll()
    {
        var posts = await _service.GetAllPostsAsync();
        return Ok(posts);
    }

    [HttpGet("published")]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPublished()
    {
        var posts = await _service.GetPublishedPostsAsync();
        return Ok(posts);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BlogPostDto>> GetById(int id)
    {
        var post = await _service.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound(new { message = $"Blog post with ID {id} not found" });
        }
        return Ok(post);
    }
    
    [HttpPost]
    public async Task<ActionResult<BlogPostDto>> Create([FromBody] CreateBlogPostDto createDto)
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
        var createDtoWithUserId = createDto with { UserId = userId.Value };

        var created = await _service.CreatePostAsync(createDtoWithUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<BlogPostDto>> Update(int id, [FromBody] UpdateBlogPostDto updateDto)
    {
        var updated = await _service.UpdatePostAsync(id, updateDto);
        if (updated == null)
        {
            return NotFound(new { message = $"Blog post with ID {id} not found" });
        }
        return Ok(updated);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _service.DeletePostAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Blog post with ID {id} not found" });
        }
        return NoContent();
    }
}
