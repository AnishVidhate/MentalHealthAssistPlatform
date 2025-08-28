using mental_health_assist_platform.DTO;
using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using mental_health_assist_platform.Services;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;
        private readonly IEmailService _emailService;
        public RegisterController(MentalHealthDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        // POST api/Register
        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterReq req)
        {
            var loginlink = "https://mental-health-assistance-lime.vercel.app/";
            if (_context.Users.Any(u => u.Email == req.Email))
            {
                return BadRequest("Email is already registered.");
            }

            var newUser = new User
            {
                Name = req.Name,
                Email = req.Email,
                Password = req.Password,  // TODO: hash it
                Role = req.Role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            bool emailSent = false;
            try
            {
                var emailRequest = new EmailRequest
                {
                    ToEmail = newUser.Email,
                    Subject = "🌿 Welcome to MindCare – Your Journey to Better Mental Well-being Begins",
                    Body = $@"
                                <div style='font-family: Arial, sans-serif; color: #333; line-height:1.6;'>
                                <h2 style='color:#4CAF50;'>Hi {newUser.Name},</h2>
                                <p>Welcome to <strong>MindCare</strong>! 🌸 We’re so glad you’ve joined our community, where your mental health and well-being truly matter.</p>
        
                                <p>Here’s what you can look forward to:</p>
                                <ul>
                                    <li>🧠 <strong>Personalized support</strong> to track and understand your moods.</li>
                                    <li>💬 <strong>Safe forums</strong> to share and connect with others.</li>
                                    <li>📚 <strong>Helpful resources</strong> to guide you toward positive mental wellness.</li>
                                    <li>👩‍⚕️ <strong>Therapy sessions</strong> with professionals when you need them most.</li>
                                </ul>
                                <p style='text-align:center; margin:30px 0;'>
                                    <a href='{loginlink}' 
                                       style='background-color:#4CAF50; color:#fff; padding:12px 24px; 
                                              text-decoration:none; border-radius:6px; font-weight:bold;'>
                                       Log In to My Account
                                    </a>
                                </p>
                                <p>Take a deep breath—you’re not alone on this journey anymore. 💙</p>

                                <p>If you didn’t sign up for MindCare, you can safely ignore this email.</p>
                                
                                <p style='margin-top:20px;'>With warmth and care,<br/><strong>The MindCare Team 🌿</strong></p>
                            </div>"
                };

                await _emailService.SendEmailAsync(emailRequest);
                emailSent = true;
            }
            catch (Exception ex)
            {
                // log the error (ex.Message)
                emailSent = false;
            }

            return Ok(new
            {
                Message = "User registered successfully.",
                EmailSent = emailSent
            });
        }

        // POST api/Register/therapist
        [HttpPost("therapist")]
        public async Task<ActionResult> RegisterTherapist([FromBody] RegisterTherapistReq req)
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
            await _context.SaveChangesAsync();

            var emailRequest = new EmailRequest
            {
                ToEmail = newTherapist.Email,
                Subject = "Thank you for registering with MindCare 🌿 – Pending Admin Approval",
                Body = $@"
                        <div style='font-family: Arial, sans-serif; color: #333; line-height:1.6;'>
                            <h2 style='color:#4CAF50;'>Hi {newTherapist.Name},</h2>
                            <p>Thank you for registering with <strong>MindCare</strong>! 🌸</p>
                            
                            <p>Your application as a therapist has been successfully received. 
                            Our admin team is currently reviewing your profile to ensure all details are accurate and meet our standards.</p>
                            
                            <p>Once approved, you will be able to:</p>
                            <ul>
                                <li>👩‍⚕️ Connect with clients seeking professional mental health assistance.</li>
                                <li>📅 Manage therapy sessions through the MindCare platform.</li>
                                <li>📚 Share your expertise and contribute to the community.</li>
                            </ul>
                            
                            <p>⏳ Please allow some time for the verification process. We’ll notify you as soon as your account is approved.</p>
                            
                            <p style='margin-top:20px;'>With gratitude,<br/><strong>The MindCare Team 🌿</strong></p>
                        </div>"

            };

            await _emailService.SendEmailAsync(emailRequest);

            return StatusCode(201, "Therapist registered successfully.");
        }

        // POST api/Register/admin
        [HttpPost("admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterReq req)
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
            await _context.SaveChangesAsync();


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
