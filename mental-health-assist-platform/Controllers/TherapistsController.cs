using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mental_health_assist_platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mental_health_assist_platform.DTO;
using mental_health_assist_platform.Services;

namespace mental_health_assist_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {
        private readonly MentalHealthDbContext _context;
        private readonly IEmailService _emailService;
        public TherapistController(MentalHealthDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            var loginlink = "https://mental-health-assistance-lime.vercel.app/";
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }

            therapist.ApprovalStatus = "approved"; // Update status 
            await _context.SaveChangesAsync();

            bool emailSent = false;
            try
            {
                var emailRequest = new EmailRequest
                {
                    ToEmail = therapist.Email,
                    Subject = "🌿 Welcome to MindCare – Your Journey to Better Mental Well-being Begins",
                    Body = $@"
                            <div style='font-family: Arial, sans-serif; color: #333; line-height:1.6;'>
                                <h2 style='color:#4CAF50;'>Hi {therapist.Name},</h2>
                                <p>Great news! 🎉 Your <strong>MindCare Therapist Account</strong> has been approved.</p>
                                
                                <p>You can now log in and start connecting with clients who need your professional support.</p>
                                
                                <p>As a therapist on MindCare, you can:</p>
                                <ul>
                                    <li>👩‍⚕️ Provide therapy sessions and consultations.</li>
                                    <li>📅 Manage appointments and schedules easily.</li
                                    <li>🌍 Make a lasting positive impact in the lives of individuals.</li>
                                </ul>
                                
                                <p style='text-align:center; margin:30px 0;'>
                                    <a href='{loginlink}' 
                                       style='background-color:#4CAF50; color:#fff; padding:12px 24px; 
                                              text-decoration:none; border-radius:6px; font-weight:bold;'>
                                       Log In to My Account
                                    </a>
                                </p>
                                
                                <p>We’re thrilled to have you on board. Together, let’s make mental health care more accessible and compassionate. 💙</p>
                                
                                <p style='margin-top:20px;'>Warm regards,<br/><strong>The MindCare Team 🌿</strong></p>
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
                Message = "Therapist Status Approved successfully.",
                EmailSent = emailSent
            });
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectTherapist(int id)
        {
            
            var therapist = await _context.Therapists.FindAsync(id);
            if (therapist == null)
            {
                return NotFound();
            }

            therapist.ApprovalStatus = "rejected"; // Update status rejected
            await _context.SaveChangesAsync();

            bool emailSent = false;
            try
            {
                var emailRequest = new EmailRequest
                {
                    ToEmail = therapist.Email,
                    Subject = "🌿 Welcome to MindCare – Your Journey to Better Mental Well-being Begins",
                    Body = $@"
                               <div style='font-family: Arial, sans-serif; color: #333; line-height:1.6;'>
                                   <h2 style='color:#E74C3C;'>Hi {therapist.Name},</h2>
                                   <p>Thank you for registering with <strong>MindCare</strong> and showing interest in joining our platform as a therapist.</p>
                                   
                                   <p>After carefully reviewing your application, we regret to inform you that your request has not been approved at this time.</p>
                                   
                                   <p>Common reasons for rejection may include:</p>
                                   <ul>
                                       <li>Incomplete or incorrect profile details.</li>
                                       <li>Missing professional credentials or verification documents.</li>
                                       <li>Not meeting MindCare’s therapist onboarding requirements.</li>
                                   </ul>
                                   
                                   <p>If you believe this decision was made in error or if you would like to re-apply after updating your details, you are welcome to submit your application again.</p>
                                   
                                   <p style='margin-top:20px;'>We truly appreciate your interest in being part of MindCare and your commitment to supporting mental health. 🌿</p>
                                   
                                   <p style='margin-top:20px;'>With gratitude,<br/><strong>The MindCare Team</strong></p>
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
                Message = "Therapist Status Approved successfully.",
                EmailSent = emailSent
            });
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


            bool emailSent = false;
            try
            {
                var emailRequest = new EmailRequest
                {
                    ToEmail = therapist.Email,
                    Subject = "🌿 Welcome to MindCare – Your Journey to Better Mental Well-being Begins",
                    Body = $@"
                               <div style='font-family: Arial, sans-serif; color: #333; line-height:1.6;'>
                                   <h2 style='color:#E74C3C;'>Hi {therapist.Name},</h2>
                                   <p>Thank you for registering with <strong>MindCare</strong> and showing interest in joining our platform as a therapist.</p>
                                   
                                   <p>After carefully reviewing your application, we regret to inform you that your request has not been approved at this time.</p>
                                   
                                   <p>Common reasons for rejection may include:</p>
                                   <ul>
                                       <li>Incomplete or incorrect profile details.</li>
                                       <li>Missing professional credentials or verification documents.</li>
                                       <li>Not meeting MindCare’s therapist onboarding requirements.</li>
                                   </ul>
                                   
                                   <p>If you believe this decision was made in error or if you would like to re-apply after updating your details, you are welcome to submit your application again.</p>
                                   
                                   <p style='margin-top:20px;'>We truly appreciate your interest in being part of MindCare and your commitment to supporting mental health. 🌿</p>
                                   
                                   <p style='margin-top:20px;'>With gratitude,<br/><strong>The MindCare Team</strong></p>
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
                Message = "Therapist Status Approved successfully.",
                EmailSent = emailSent
            });
        }
    }
}
