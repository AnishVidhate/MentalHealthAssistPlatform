using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;

        public RegisterController(MentalHealthDbContext context) => _context = context;

        // POST api/Register
        [HttpPost]
        public ActionResult<User> RegisterUser([FromBody] RegisterReq req)
        {
            if (_context.Users.Any(u => u.Email == req.Email))
            {
                return BadRequest("Email is already registered.");
            }

            var newUser = new User
            {
                Name = req.Name,
                Email = req.Email,
                Password = req.Password,  // 🔴 TODO: Hash the password before saving
                Role = req.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return StatusCode(201, "User registered successfully.");
        }

        // POST api/Register/therapist
        [HttpPost("therapist")]
        public ActionResult<Therapist> RegisterTherapist([FromBody] RegisterTherapistReq req)
        {
            if (_context.Therapists.Any(t => t.Email == req.Email))
            {
                return BadRequest("Email is already registered.");
            }

            var newTherapist = new Therapist
            {
                Name = req.Name,
                Email = req.Email,
                Password = req.Password,  // 🔴 TODO: Hash the password before saving
                Role = "therapist",
                Specialization = req.Specialization,
                YearsOfExperience = req.YearsOfExperience,
                LicenseNumber = req.LicenseNumber
            };

            _context.Therapists.Add(newTherapist);
            _context.SaveChanges();

            return StatusCode(201, "Therapist registered successfully.");
        }

        // POST api/Register/admin
        [HttpPost("admin")]
        public ActionResult<User> RegisterAdmin([FromBody] RegisterReq req)
        {
            if (_context.Users.Any(u => u.Email == req.Email))
            {
                return BadRequest("Email is already registered.");
            }

            var newAdmin = new User
            {
                Name = req.Name,
                Email = req.Email,
                Password = req.Password,  // 🔴 TODO: Hash the password before saving
                Role = "admin"
            };

            _context.Users.Add(newAdmin);
            _context.SaveChanges();

            return StatusCode(201, "Admin registered successfully.");
        }
    }

    // DTO for Normal User & Admin
    public class RegisterReq
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";  // Default role is "user"
    }

    // DTO for Therapists
    public class RegisterTherapistReq : RegisterReq
    {
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public string LicenseNumber { get; set; }
    }
}
