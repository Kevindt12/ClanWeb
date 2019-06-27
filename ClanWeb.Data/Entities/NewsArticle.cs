using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanWeb.Data.Entities
{

    public class NewsArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ShortText { get; set; }

        public string LongText { get; set; }

        public DateTime Date { get; set; }


        [Required]
        public virtual User CreatedBy { get; set; }


    }



}

