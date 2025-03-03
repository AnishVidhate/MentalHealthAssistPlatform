using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionControllerCheck : ControllerBase
{
    private readonly MentalHealthDbContext _context;

    public SubscriptionControllerCheck(MentalHealthDbContext context)
    {
        _context = context;
    }

    // ✅ Get subscription status for a specific user (NO SESSION)
    [HttpGet("check/{userId}")]
    public async Task<ActionResult<bool>> GetSubscriptionStatus(int userId)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);

        // Ensure subscription is not null before accessing SubscriptionStatus
        return Ok(subscription?.SubscriptionStatus ?? false);
    }
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<object>> CheckUserSubscription(int userId)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.SubscriptionStatus == true);

        if (subscription == null)
            return Ok(new { isActive = false });

        return Ok(new { isActive = true });
    }



    // ✅ Create a new subscription (For initial sign-up) - No session-based authentication
    [HttpPost("create/{userId}")]
    public async Task<ActionResult> CreateSubscription(int userId)
    {
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

    // ✅ Upgrade Subscription (For Razorpay Payment Success) - No session-based authentication
    [HttpPatch("upgrade/{userId}")]
    public async Task<IActionResult> UpgradeSubscription(int userId)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (subscription == null)
            return NotFound("Subscription not found.");

        subscription.SubscriptionStatus = true;
        subscription.LastPaymentDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok("Subscription upgraded successfully.");
    }

    // ✅ Downgrade Subscription (For cancellation) - No session-based authentication
    [HttpPatch("downgrade/{userId}")]
    public async Task<IActionResult> DowngradeSubscription(int userId)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        if (subscription == null)
            return NotFound("Subscription not found.");

        subscription.SubscriptionStatus = false;
        await _context.SaveChangesAsync();

        return Ok("Subscription downgraded successfully.");
    }
}
