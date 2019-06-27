using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClanWeb.Data.Entities.Forum
{

    // Messages
    public class Message
    {
        public Message()
        {
            UsersThatVoted = new List<ForumUser>();
        }

        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool Deleted { get; set; }

        public int Reputation { get; set; }

        public int MessagePlacementInThread { get; set; }


        // Releations to other models
        [Required]
        public virtual Thread Thread { get; set; }

        public virtual ForumUser User { get; set; }

        public virtual ICollection<ForumUser> UsersThatVoted { get; set; }

    }
}
