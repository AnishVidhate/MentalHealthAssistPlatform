using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateForumsController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;

        public CreateForumsController(MentalHealthDbContext context) => _context = context;

        // Get all forums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forum>>> GetAllForums()
        {
            var forums = await _context.Forums.ToListAsync();
            return Ok(forums);
        }

        // Get a single forum by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Forum>> GetForumById(int id)
        {
            var forum = await _context.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound("Forum not found.");
            }
            return Ok(forum);
        }

        // Create a new forum
        [HttpPost]
        public async Task<ActionResult<Forum>> CreateForum([FromBody] ForumReq req)
        {
            if (string.IsNullOrEmpty(req.Title) || string.IsNullOrEmpty(req.Description))
            {
                return BadRequest("Title and description are required.");
            }

            var newForum = new Forum
            {
                Title = req.Title,
                Description = req.Description,
                CreatedBy = req.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _context.Forums.Add(newForum);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForumById), new { id = newForum.Id }, newForum);
        }

        // Update a forum
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForum(int id, [FromBody] ForumReq req)
        {
            var forum = await _context.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound("Forum not found.");
            }

            forum.Title = req.Title;
            forum.Description = req.Description;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a forum
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            var forum = await _context.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound("Forum not found.");
            }

            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class ForumReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
    }
}
