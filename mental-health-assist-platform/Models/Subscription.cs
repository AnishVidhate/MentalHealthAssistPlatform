using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool? SubscriptionStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }

    }
}
