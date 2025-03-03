using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly MentalHealthDbContext _context;

    public SubscriptionController(MentalHealthDbContext context)
    {
        _context = context;
    }

    // Get subscription status for logged-in user
    [HttpGet("status")]
    [Authorize] // Ensures user is logged in
    public async Task<ActionResult<bool>> GetSubscriptionStatus()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (subscription == null || subscription.SubscriptionStatus == false)
            return Ok(false); // User is not subscribed

        return Ok(true); // User is subscribed
    }

    // Create a new subscription (For initial sign-up)
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateSubscription()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var existingSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (existingSubscription != null)
            return BadRequest("User already has a subscription.");

        var subscription = new Subscription
        {
            UserId = userId,
            SubscriptionStatus = true,
            LastPaymentDate = DateTime.Now
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return Ok("Subscription created successfully.");
    }

    // Upgrade Subscription (For Razorpay Payment Success)
    [HttpPatch("upgrade")]
    [Authorize]
    public async Task<IActionResult> UpgradeSubscription()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (subscription == null)
            return NotFound("Subscription not found.");

        subscription.SubscriptionStatus = true;
        subscription.LastPaymentDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok("Subscription upgraded successfully.");
    }

    // Downgrade Subscription (For cancellation)
    [HttpPatch("downgrade")]
    [Authorize]
    public async Task<IActionResult> DowngradeSubscription()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (subscription == null)
            return NotFound("Subscription not found.");

        subscription.SubscriptionStatus = false;
        await _context.SaveChangesAsync();

        return Ok("Subscription downgraded successfully.");
    }
}