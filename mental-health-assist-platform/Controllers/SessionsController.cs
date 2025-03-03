using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly MentalHealthDbContext _context;
    public SessionsController(MentalHealthDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Session>>> GetSessions() => await _context.Sessions.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Session>> GetSession(int id)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null) return NotFound();
        return session;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Session>>> GetUserSessions(int userId)
    {
        var sessions = await _context.Sessions
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (sessions == null || sessions.Count == 0)
            return NotFound("No upcoming sessions found.");

        return sessions;
    }


    [HttpPost("create")]
    public async Task<ActionResult<Session>> CreateSession([FromBody] Session session)
    {
        if (session.UserId <= 0 || session.TherapistId <= 0)
            return BadRequest("Invalid user or therapist ID.");

        var therapist = await _context.Therapists.FindAsync(session.TherapistId);
        if (therapist == null)
            return NotFound("Therapist not found.");

        session.TherapistName = therapist.Name;
        session.DateTime = DateTime.Now;
        session.Status = "Pending"; // Default status when created

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
    }

    [HttpPut("schedule/{id}")]
    public async Task<IActionResult> ScheduleSession(int id, [FromBody] Session updatedSession)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null)
            return NotFound(new { message = "Session not found." });

        // Debugging: Log received data
        Console.WriteLine($"Received Session Data: Id={id}, DateTime={updatedSession.DateTime}, Status={updatedSession.Status}, UserId={updatedSession.UserId}, TherapistId={updatedSession.TherapistId}");

        if (updatedSession.DateTime == default)
            return BadRequest(new { message = "Invalid DateTime format received." });

        if (updatedSession.UserId <= 0 || updatedSession.TherapistId <= 0)
            return BadRequest(new { message = "UserId or TherapistId is invalid." });

        // Update session details
        session.DateTime = updatedSession.DateTime;
        session.Status = "Scheduled";

        _context.Entry(session).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Session scheduled successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
        }
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null) return NotFound();
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
