using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClanWeb.Web.Models
{
    public class UserCardViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string SteamUserName { get; set; }

        public string Seniority { get; set; }

        public string Country { get; set; }

        public string Role { get; set; }

        public string Image { get; set; }
    }

    public class MembersViewModel
    {
        public MembersViewModel()
        {
            Administrators = new List<UserCardViewModel>();
            Moderators = new List<UserCardViewModel>();
            Members = new List<UserCardViewModel>();
        }

        public IEnumerable<UserCardViewModel> Administrators { get; set; }

        public IEnumerable<UserCardViewModel> Moderators { get; set; }

        public IEnumerable<UserCardViewModel> Members { get; set; }


    }


    public class NewsArticlePreviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TextArea { get; set; }       

        public DateTime Date { get; set; }
    }


    public class NewsArticleViewModel
    {
        public string Title { get; set; }

        public string CreatedBy { get; set; }


        public string TextArea { get; set; }        

        public DateTime Date { get; set; }

    }

    public class NewsArticlesListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Published { get; set; }

        public string CreatedBy { get; set; }


    }

    public class EventListingViewModel
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }
    }


    public class EventViewModel
    {
        public string EventName { get; set; }

        public string EventDateAndTime { get; set; }

        public string Game { get; set; }

        public string Location { get; set; }

        public string CreatedBy { get; set; }

        public string Description { get; set; }
        
        public string ImageLocation { get; set; }

    }



    
}