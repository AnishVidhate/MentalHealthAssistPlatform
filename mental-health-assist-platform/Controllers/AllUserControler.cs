using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalHealthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllUserController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;

        public AllUserController(MentalHealthDbContext context)
        {
            _context = context;
        }

        // ✅ Get all users whose role is not 'admin'
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.Role != "admin") // Filter out admin users
                .ToListAsync();

            return Ok(users);
        }

        // ✅ Get all therapists
        [HttpGet("therapists")]
        public async Task<ActionResult<IEnumerable<Therapist>>> GetTherapists()
        {
            var therapists = await _context.Therapists.ToListAsync();
            return Ok(therapists);
        }

        // ✅ Get user by ID (only for non-admin users)
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role == "admin")
            {
                return NotFound("User not found or is an admin.");
            }
            return Ok(user);
        }

        // ✅ Get therapist by ID
        [HttpGet("therapists/{id}")]
        public async Task<ActionResult<Therapist>> GetTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound("Therapist not found.");
            }
            return Ok(therapist);
        }

        // ✅ Delete a user (excluding admins)
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role == "admin")
            {
                return NotFound("User not found or cannot be deleted because they are an admin.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User deleted successfully." });
        }

        // ✅ Delete a therapist
        [HttpDelete("therapists/{id}")]
        public async Task<IActionResult> DeleteTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound("Therapist not found.");
            }

            _context.Therapists.Remove(therapist);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Therapist deleted successfully." });
        }
    }
}
