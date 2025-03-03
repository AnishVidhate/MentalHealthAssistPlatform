using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TherapistId { get; set; }
        public string TherapistName { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string? Status { get; set; }

/*        public virtual Therapist Therapist { get; set; } = null!;
        public virtual User User { get; set; } = null!;*/
    }
}
