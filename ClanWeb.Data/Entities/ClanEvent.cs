using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanWeb.Data.Entities
{
    public class ClanEvent
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime EventDateAndTime { get; set; }

        public string Game { get; set; }

        public string Location { get; set; }

        public string ImageLocation { get; set; }

        public string Description { get; set; }


        [Required]
        public virtual User CreatedBy { get; set; }
    }
}
