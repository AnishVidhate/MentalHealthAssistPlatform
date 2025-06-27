using mental_health_assist_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;
using System.Collections.Generic;

namespace PaymentApp.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly MentalHealthDbContext _context;

        public PaymentController(MentalHealthDbContext context)
        {
            _context = context;
        }

        private readonly string _key = "rzp_test_rZfdStCvfRhsxH";
        private readonly string _secret = "TXPNbYMzSvwnYlQ2bBUx8DhJ";

        [HttpPost("createorder")]
        public IActionResult CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                if (request == null || request.UserId <= 0 || request.Amount <= 0 || string.IsNullOrEmpty(request.Currency))
                {
                    return BadRequest(new { message = "Invalid request data. Please provide valid UserId, Amount, and Currency." });
                }

                //  Create Razorpay Order
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", request.Amount * 100 },  // ✅ Convert rupees to paisa
                    { "currency", request.Currency },
                    { "receipt", $"order_rcptid_{Guid.NewGuid().ToString().Substring(0, 8)}" }
                };

                Order order = client.Order.Create(options);
                return Ok(new { orderId = order["id"].ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating order: " + ex.Message });
            }
        }



        [HttpPost("savepayment")]
        public async Task<IActionResult> SavePayment([FromBody] mental_health_assist_platform.Models.Payment payment)
        {
            if (payment == null || payment.UserId <= 0 || payment.Amount <= 0 || string.IsNullOrEmpty(payment.OrderId))
            {
                return BadRequest(new { message = "Invalid payment data." }); // ✅ Input validation
            }

            try
            {
                //  Check if UserId exists in the Users table to avoid foreign key errors
                var userExists = await _context.Users.AnyAsync(u => u.Id == payment.UserId);
                if (!userExists)
                {
                    return BadRequest(new { message = "Invalid UserId. User does not exist." });
                }

                //  Check if a payment with the same ID already exists (only if Id is manually set)
                var existingPayment = await _context.Payments.FindAsync(payment.Id);
                if (existingPayment != null)
                {
                    return Conflict(new { message = "Payment ID already exists." });
                }

                //  Save the payment first
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                //  Check if the user already exists in the Subscription table
                var existingSubscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == payment.UserId);

                if (existingSubscription == null)
                {
                    var subscription = new mental_health_assist_platform.Models.Subscription
                    {
                        UserId = payment.UserId,
                        SubscriptionStatus = true,
                        LastPaymentDate = DateTime.UtcNow
                    };

                    _context.Subscriptions.Add(subscription);
                }
                else
                {
                    // If the user already exists, just update the LastPaymentDate
                    existingSubscription.LastPaymentDate = DateTime.UtcNow;
                    existingSubscription.SubscriptionStatus = true;

                    _context.Subscriptions.Update(existingSubscription);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Payment and subscription saved successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error saving payment.",
                    error = ex.InnerException?.Message ?? ex.Message // ✅ Captures detailed error message
                });
            }
        }




    }

    public class OrderRequest
    {
        public int UserId { get; set; } 
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
