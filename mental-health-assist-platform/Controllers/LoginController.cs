using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;
        public LoginController(MentalHealthDbContext context) => _context = context;

        [HttpPost]
        public ActionResult<object> Verify([FromBody] LoginReq req)
        {
            // Check in Users table
            var user = _context.Users.FirstOrDefault(a => a.Email == req.email);

            if (user != null)
            {
                if (user.Password == req.password)
                {
                    return Ok(new
                    {
                        id = user.Id,
                        name = user.Name,
                        email = user.Email,
                        role = user.Role
                    });
                }
                return Unauthorized("Invalid password.");
            }

            // Check in Therapists table
            var therapist = _context.Therapists.FirstOrDefault(t => t.Email == req.email);

            if (therapist != null)
            {
                if (therapist.Password == req.password)
                {
                    if (therapist.ApprovalStatus?.ToLower() != "approved")
                    {
                        return Unauthorized("Your account has not been approved yet.");
                    }

                    return Ok(new
                    {
                        id = therapist.Id,
                        name = therapist.Name,
                        email = therapist.Email,
                        role = "therapist",
                        specialization = therapist.Specialization,
                        experience = therapist.YearsOfExperience
                    });
                }
                return Unauthorized("Invalid password.");
            }

            return BadRequest("User not found.");
        }
    }

    public class LoginReq
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}