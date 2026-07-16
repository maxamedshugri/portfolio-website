using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortfolioApi.Data;
using PortfolioApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Username and password are required." });

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid username or password." });

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    // Protected endpoint - lets the logged-in admin change their username and/or password
    [HttpPut("update-credentials")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> UpdateCredentials([FromBody] UpdateCredentialsDto dto)
    {
        var currentUsername = User.Identity?.Name;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);
        if (user == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || !BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            return BadRequest(new { message = "Current password is incorrect." });

        if (!string.IsNullOrWhiteSpace(dto.NewUsername))
        {
            var taken = await _db.Users.AnyAsync(u => u.Username == dto.NewUsername && u.Id != user.Id);
            if (taken) return BadRequest(new { message = "That username is already taken." });
            user.Username = dto.NewUsername;
        }

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            if (dto.NewPassword.Length < 6)
                return BadRequest(new { message = "New password must be at least 6 characters." });
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        }

        await _db.SaveChangesAsync();

        // Issue a fresh token in case the username changed
        var token = GenerateJwtToken(user);
        return Ok(new { message = "Credentials updated.", token });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiryMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
