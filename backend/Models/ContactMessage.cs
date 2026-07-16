namespace PortfolioApi.Models;

public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// DTO used when the public contact form submits a new message
public class ContactMessageCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

// DTO used for the login request
public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// DTO used when the admin changes their username/password
public class UpdateCredentialsDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string? NewUsername { get; set; }
    public string? NewPassword { get; set; }
}
