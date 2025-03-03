using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class Chat
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;

        public virtual Forum Forum { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
