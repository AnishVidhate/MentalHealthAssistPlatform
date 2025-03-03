using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class Therapist
    {
        public Therapist()
        {
            /*Sessions = new HashSet<Session>();*/
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public int? YearsOfExperience { get; set; }
        public string LicenseNumber { get; set; } = null!;
        public string? ApprovalStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Role { get; set; } = null!;

 /*       public virtual ICollection<Session> Sessions { get; set; }*/
    }
}
