using System;
using System.Collections.Generic;

namespace mental_health_assist_platform.Models
{
    public partial class Forum
    {
        public Forum()
        {
         /*   Chats = new HashSet<Chat>();*/
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

/*        public virtual User CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<Chat> Chats { get; set; }*/
    }
}
