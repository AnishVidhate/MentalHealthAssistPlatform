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
        var chats = await _context.Chats.Where(c=>c.ForumId ==id).ToListAsync();
        if (chats == null) return NotFound();
        return Ok(chats);
    }

    [HttpPost]
    public async Task<ActionResult<Chat>> CreateChat(ChatCreateDto chatDto)
    {
        Console.WriteLine("chatDto" + chatDto);
        var user = await _context.Users.FindAsync(chatDto.UserId);
        var forum = await _context.Forums.FindAsync(chatDto.ForumId);

        if (user == null || forum == null)
        {
            return BadRequest("Invalid user or forum ID.");
        }

        var chat = new Chat
        {
            ForumId = chatDto.ForumId,
            UserId = chatDto.UserId,
            Message = chatDto.Message
        };

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
public class ChatCreateDto
{
    public int ForumId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
}

