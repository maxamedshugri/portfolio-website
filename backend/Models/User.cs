using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(95)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
