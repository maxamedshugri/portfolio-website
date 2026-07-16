using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _db;

    public ContactController(AppDbContext db)
    {
        _db = db;
    }

    // Public endpoint - called from index.html contact form
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] ContactMessageCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest(new { message = "Name, email and message are required." });

        var entity = new ContactMessage
        {
            Name = dto.Name,
            Email = dto.Email,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.ContactMessages.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Message received." });
    }

    // Protected endpoint - called from admin.html
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var messages = await _db.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPut("{id}/read")]
    [Authorize]
    public async Task<IActionResult> MarkRead(int id)
    {
        var msg = await _db.ContactMessages.FindAsync(id);
        if (msg == null) return NotFound();

        msg.IsRead = true;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var msg = await _db.ContactMessages.FindAsync(id);
        if (msg == null) return NotFound();

        _db.ContactMessages.Remove(msg);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
