using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClanWeb.Data.Entities.Forum
{
    public class Thread
    {
        public Thread()
        {
            Messages = new List<Message>();
            Subscribers = new List<ForumUser>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ViewCount { get; set; }

        public string[] Tags { get; set; }

        public DateTime LastEdited { get; set; }

        public bool Pinned { get; set; }

        public int Reputation { get; set; }


        // Other model relations
        public virtual ICollection<Message> Messages { get; set; }

        [Required]
        public virtual Group Group { get; set; }

        public virtual ForumUser User { get; set; }

        public virtual ICollection<ForumUser> Subscribers { get; set; }
    }
}
