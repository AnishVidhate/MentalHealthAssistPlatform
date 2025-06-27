using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistControllerEdit : ControllerBase
    {
        private readonly MentalHealthDbContext _context;
        public TherapistControllerEdit(MentalHealthDbContext context) => _context = context;

        //  Get all therapists
        [HttpGet]
        public ActionResult<IEnumerable<Therapist>> GetAllTherapists()
        {
            return Ok(_context.Therapists.ToList());
        }

        //  Get only pending therapists
        [HttpGet("pending")]
        public ActionResult<IEnumerable<Therapist>> GetPendingTherapists()
        {
            var pendingTherapists = _context.Therapists
                .Where(t => t.ApprovalStatus.ToLower() == "pending")
                .ToList();

            return Ok(pendingTherapists);
        }

        //  Approve therapist
        [HttpPut("{id}/approve")]
        public IActionResult ApproveTherapist(int id)
        {
            var therapist = _context.Therapists.FirstOrDefault(t => t.Id == id);

            if (therapist == null)
                return NotFound("Therapist not found.");

            therapist.ApprovalStatus = "Approved";
            _context.SaveChanges();

            return Ok(new { message = "Therapist approved successfully." });
        }

        //  Reject (delete) therapist
        [HttpDelete("{id}")]
        public IActionResult RejectTherapist(int id)
        {
            var therapist = _context.Therapists.FirstOrDefault(t => t.Id == id);

            if (therapist == null)
                return NotFound("Therapist not found.");

            _context.Therapists.Remove(therapist);
            _context.SaveChanges();

            return Ok(new { message = "Therapist rejected successfully." });
        }

  
    }
}
