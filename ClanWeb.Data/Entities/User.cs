using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ClanWeb.Data.Entities
{
    // User model
    public class User : IdentityUser
    {
        public User() : base()
        {

        }

        // Sets the level of the person
        public string RankLevel { get; set; }

        // From country your form

        [MaxLength(40)]
        public string Country { get; set; }

        public string Seniority { get; set; }

        public string PrimaryRole { get; set; }

        public bool IsInReview { get; set; }

        


    }
}
