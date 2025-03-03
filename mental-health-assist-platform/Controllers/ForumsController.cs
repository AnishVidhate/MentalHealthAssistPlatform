using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ForumsController : ControllerBase
{
    private readonly MentalHealthDbContext _context;

    public ForumsController(MentalHealthDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Forum>>> GetForums()
    {
        return await _context.Forums.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Forum>> GetForum(int id)
    {
        var forum = await _context.Forums.FindAsync(id);
        if (forum == null) return NotFound();
        return forum;
    }

    [HttpPost]
    public async Task<ActionResult<Forum>> CreateForum(Forum forum)
    {
        _context.Forums.Add(forum);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetForum), new { id = forum.Id }, forum);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateForum(int id, Forum forum)
    {
        if (id != forum.Id) return BadRequest();
        _context.Entry(forum).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteForum(int id)
    {
        var forum = await _context.Forums.FindAsync(id);
        if (forum == null) return NotFound();
        _context.Forums.Remove(forum);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
