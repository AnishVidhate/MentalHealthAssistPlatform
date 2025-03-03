using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly MentalHealthDbContext _context;
    public ChatsController(MentalHealthDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Chat>>> GetChats() => await _context.Chats.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Chat>> GetChat(int id)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null) return NotFound();
        return chat;
    }

    [HttpPost]
    public async Task<ActionResult<Chat>> CreateChat(Chat chat)
    {
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChat(int id, Chat chat)
    {
        if (id != chat.Id) return BadRequest();
        _context.Entry(chat).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChat(int id)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat == null) return NotFound();
        _context.Chats.Remove(chat);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
