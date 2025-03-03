using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class User
    {
        public User()
        {
/*            Chats = new HashSet<Chat>();
            Forums = new HashSet<Forum>();
            Payments = new HashSet<Payment>();
            Sessions = new HashSet<Session>();
            Subscriptions = new HashSet<Subscription>();*/
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; }
/*
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<Forum> Forums { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }*/
    }
}
