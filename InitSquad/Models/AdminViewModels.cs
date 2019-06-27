using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ClanWeb.Web.Models
{

    public class UsersViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Seniority")]
        public string Seniority { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }


    public class ChangeRoleViewModel
    {
        // Hidden in view
        public string UserId { get; set; }

        [Display(Name = "Role")]
        public string PrimaryRole { get; set; }

        public List<Role> Roles { get; set; }

        public ChangeRoleViewModel()
        {
            Roles = new List<Role>();
        }

    }



    public class Role
    {

        public string Name { get; set; }

        public bool Checked { get; set; }

    }




    public class ANewsArticlesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string UserNameOfPublisher { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }

    public class AEventsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

    }




    /////////////////////////////// FORMS ////////////////////////////////////////



    public class CreateNewsArticleViewModel
    {
        [Required]
        [Display(Name = "Title")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public string Title { get; set; }

    

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string LongText { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public DateTime Date { get; set; }

    }

    public class EditNewsArticleViewModel
    {
        // Not visible
        [Required]
        [Display(Name = "Title")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public string Title { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string ShortText { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string LongText { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public DateTime Date { get; set; }

    }
    
    public class CreateEventViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "That is too many caracters")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public DateTime EventDate { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string Game { get; set; }

        [Required]
        public string Location { get; set; }


    }

    public class EditEventViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "That is too many caracters")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public DateTime EventDate { get; set; }

        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

        public string OrginalImageUrl { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string Game { get; set; }

        [Required]
        public string Location { get; set; }



    }

    public class EditUserViewModel
    {
        // Hidden in view
        public string UserId { get; set; }

        [Display(Name = "Seniorty")]
        [RegularExpression("[ -~]+", ErrorMessage = "Please use only printable English characters")]
        public string Seniority { get; set; }

    }

    public class GenerateCodeViewModel
    {
        [Required]
        [MinLength(12, ErrorMessage = "The Code has to be more than 12 characters")]
        public string Code { get; set; }

        public string ReferalLink { get; set; }

       
    }



    public class ApprovalUsersListViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string SteamUrl { get; set; }

        public DateTime? Applied { get; set; }

        public string DiscordUrl { get; set; }

    }

}