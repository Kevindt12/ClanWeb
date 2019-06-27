using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanWeb.Data.Entities.Forum
{
    // Users Extended forums userdatabase
    public class ForumUser
    {

        public ForumUser()
        {
            Subscriptions = new List<Thread>();
            PostedMessages = new List<Message>();
            VotedMessages = new List<Message>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public int Reputation { get; set; }

        public int PostsCount { get; set; }

        public string Signature { get; set; }

        public DateTime LastActivity { get; set; }

        public DateTime JoinDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Thread> Subscriptions { get; set; }

        public virtual ICollection<Message> PostedMessages { get; set; }

        public virtual ICollection<Message> VotedMessages { get; set; }

    }
}
