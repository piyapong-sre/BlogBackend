using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BlogBackend.DTOs;
using BlogBackend.Models;
using BlogBackend.Repositories;

namespace BlogBackend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

        if (user == null || !user.IsActive)
            return null;

        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            return null;

        var token = GenerateJwtToken(user.Id, user.Username, user.Email);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            ExpiresAt = expiresAt
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsAsync(registerDto.Username, registerDto.Email))
            return null;

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = HashPassword(registerDto.Password),
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);
        var token = GenerateJwtToken(createdUser.Id, createdUser.Username, createdUser.Email);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResponseDto
        {
            Token = token,
            Username = createdUser.Username,
            Email = createdUser.Email,
            DisplayName = createdUser.DisplayName,
            ExpiresAt = expiresAt
        };
    }

    public string GenerateJwtToken(int userId, string username, string email)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
