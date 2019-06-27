using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClanWeb.Data.Entities.Forum
{

    public class Group
    {
        public Group()
        {
            Threads = new List<Thread>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Category { get; set; }


        
        public virtual ICollection<Thread> Threads { get; set; }

    }
}
