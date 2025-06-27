using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mental_health_assist_platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;

        public TherapistController(MentalHealthDbContext context)
        {
            _context = context;
        }

        //  GET: api/Therapist (Get all therapists)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Therapist>>> GetTherapists()
        {
            return await _context.Therapists.ToListAsync(); // Fetches all therapists
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Therapist>>> GetPendingTherapists()
        {
            return await _context.Therapists
                                 .Where(t => t.ApprovalStatus.ToLower() == "pending") // Ensure case-insensitive match
                                 .ToListAsync();
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }

            therapist.ApprovalStatus = "approved"; // Update status
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //  GET: api/Therapist/{id} (Get therapist by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Therapist>> GetTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }
            return therapist;
        }

        //  PUT: api/Therapist/{id} (Update therapist profile)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTherapist(int id, Therapist updatedTherapist)
        {
            if (id != updatedTherapist.Id)
            {
                return BadRequest("Therapist ID mismatch");
            }

            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }

            // Update only the fields that can be changed
            therapist.Name = updatedTherapist.Name;
            therapist.Specialization = updatedTherapist.Specialization;
            therapist.YearsOfExperience = updatedTherapist.YearsOfExperience;
            therapist.LicenseNumber = updatedTherapist.LicenseNumber;
            therapist.ApprovalStatus = updatedTherapist.ApprovalStatus;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while updating the profile.");
            }
        }

        //  POST: api/Therapist (Register a new therapist)
        [HttpPost]
        public async Task<ActionResult<Therapist>> CreateTherapist(Therapist therapist)
        {
            if (_context.Therapists.Any(t => t.Email == therapist.Email))
            {
                return Conflict("Email already exists");
            }

            therapist.CreatedAt = DateTime.UtcNow;
            therapist.Role = "Therapist"; // Default role
            _context.Therapists.Add(therapist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTherapist), new { id = therapist.Id }, therapist);
        }

        //  DELETE: api/Therapist/{id} (Delete therapist)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTherapist(int id)
        {
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }

            _context.Therapists.Remove(therapist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
