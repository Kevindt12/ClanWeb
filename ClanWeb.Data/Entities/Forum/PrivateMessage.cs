using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanWeb.Data.Entities.Forum
{
    // Private Messages
    public class PrivateMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime SendTime { get; set; }

        // Other model relations
        public virtual ForumUser To { get; set; }

        public virtual ForumUser From { get; set; }
    }
}
