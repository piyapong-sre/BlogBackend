using Microsoft.AspNetCore.Mvc;
using BlogBackend.DTOs;
using BlogBackend.Services;

namespace BlogBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        if (result == null)
            return BadRequest(new { message = "Username or email already exists" });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(result);
    }
}
