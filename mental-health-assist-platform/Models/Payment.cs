using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mental_health_assist_platform.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow; // ✅ Defaults to current UTC time

        public string? ReferenceId { get; set; } // ✅ Stores Razorpay Payment ID

        [Required]
        [Column("order_id")]
        public string OrderId { get; set; } // ✅ Stores Razorpay Order ID
    }
}
