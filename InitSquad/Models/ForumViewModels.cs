using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ClanWeb.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Drawing;
using PagedList;

namespace ClanWeb.Web.Models
{

    
    //////////////////////////////////// Display View Models /////////////////////////////////////
    

    public class GroupSetViewModel
    {

        public GroupSetViewModel()
        {
            this.Groups = new List<GroupViewModel>();
        }

        public string Name { get; set; }

        public ICollection<GroupViewModel> Groups { get; set; }
    }

    // Subclasses
    public class GroupViewModel
    {

        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Threads")]
        public int ThreadCount { get; set; }

        [Display(Name = "Posts")]
        public int PostCount { get; set; }

        public string LastPostedThreadName { get; set; }

        public string LastPostedThreadUserName { get; set; }

        public string Image { get; set; }
        public int Id { get; internal set; }
    }







    public class ThreadsViewModel
    {
        public int Id { get; set; }

        public int Vote { get; set; }

        public string Name { get; set; }

        public string StartedTimeStamp { get; set; }

        public int NumberOfMessages { get; set; }

        public int ViewCount { get; set; }

        public string StartedByUserName { get; set; }

        public string LastMessageUserName { get; set; }

        public DateTime? LastMessageTimeStamp { get; set; }


    }

    public class ThreadViewModel
    {
        public ThreadViewModel()
        {

        }


        public string ThreadName { get; set; }

        public bool Subscribed { get; set; }


        /// <summary>
        /// Thease are message id
        /// </summary>
        public IPagedList<MessagesViewModel> Messages;
    }

    public class MessagesViewModel
    {
        // Thease are redirect ID
        public int GroupId { get; set; }

        public int ThreadId { get; set; }

        public string Image { get; set; }

        public int MessageNumber { get; set; }

        public DateTime MessagePost { get; set; }

        public string Body { get; set; }

        public string UserName { get; set; }

        public string SteamUserName { get; set; }

        public string Country { get; set; }

        public string SecondCountry { get; set; }

        public bool IsCreator { get; set; }

        public DateTime JoinDate { get; set; }

        public int Posts { get; set; }

        public int Reputation { get; set; }

        public bool Deleted { get; set; }

        public bool VotingEnabled { get; set; }

        public string Signiture { get; set; }

        public int Id { get; internal set; }
    }



    public class ForumProfleViewModel
    { 
        public string UserName { get; set; }

        public int Reputation { get; set; }

        public int PostCount { get; set; }

        public string ProfilePicture { get; set; }

        public string BaseRole { get; set; }
    }

    public class LastActuvityThreadsViewModel
    {

        public string TheadName { get; set; }

        public int ThreadId { get; set; }

        public int GroupId { get; set; }

        public DateTime DateOfActivery { get; set; }

    }

    public class SubscriptionsThreadsViewModel
    {

        public string ThreadName { get; set; }

        public int ThreadId { get; set; }

        public int GroupId { get; set; }

        public string LastUserThatAddedAMessage { get; set; }

        public DateTime LastEditedToThread { get; set; }

    }


    public class UserProfileViewModel
    {
        public string UserName { get; set; }

        public string Image { get; set; }

        public string SteamUserName { get; set; }

        public String SteamUrl { get; set; }

        public int Reputation { get; set; }

        public int Postcount { get; set; }

        public string Role { get; set; }

        public DateTime? LastActivity { get; set; }

        public DateTime JoinDate { get; set; }

        public string Country { get; set; }

        public string SecondCountry { get; set; }

        public string Seniorty { get; set; }

        public string Description { get; set; }
    }



    //////////////////////////////////// Form View Model /////////////////////////////////////




    // View : AddGroup | Controller : Forum/AddGroup
    public class AddGroupViewModel
    {
        [Display(Name = "Name")]
        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Required]
        public string Description { get; set; }

        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Display(Name = "Group Set")]
        public string Category { get; set; }

        public HttpPostedFileBase Image { get; set; }

    }

    public class EditGroupViewModel
    {
        [Display(Name = "Name")]
        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Group Set")]
        public string Category { get; set; }

        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        [Display(Name = "A new Group Set")]
        public string NewGroupSet { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string ImageUrl { get; set; }
    }

    // View : AddThread | Controller : Forum/AddThread
    public class AddThreadViewModel
    {
        [Display(Name = "The Title")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Message")]
        [Required]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string FirstMessage { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Pinned")]
        public bool Pinned { get; set; }


    }


    public class EditThreadViewModel
    {
        [Display(Name = "The Title")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }
    }


    

    // View : AddMessage | Controller : Forum/AddMessage
    public class AddMessageViewModel
    {
        public int ThreadId { get; set; }

        public int GroupId { get; set; }


        [Display(Name = "Message")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Required]
        public string Body { get; set; }
    }

    // View: EditMessage | Controller: Forum/EditMessage
    public class EditMessageViewModel
    {
        [Display(Name = "Body")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [Required]
        public string Body { get; set; }
        
    }

    public class EditThreadNameViewModel
    {
        [Display(Name = "Name")]
        [Required]
        [MinLength(length: 6)]
        [MaxLength(length: 80)]
        public string Name { get; set; }
    }

    public class AddOrEditTagsViewModel
    {
        [Display(Name = "Tags")]
        public string Tags { get; set; }

    }

    
}