using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ClanWeb.Web.Models;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Configuration;

using ClanWeb.Core.Identity;
using ClanWeb.Data.Entities;
using ClanWeb.Core.Infomation;
using System.Threading.Tasks;
using PagedList;

namespace ClanWeb.Web.Controllers
{


    public class HomeController : Controller
    {
        private ClanEventManager _clanEventManager;
        private NewsManager _newsManager;
        private RoleManager _roleManager;
        private UserManager _userManager;
        private IProfilePictureSystem _profilePictureSystem;


        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<RoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public NewsManager NewsManager
        {
            get
            {
                return _newsManager ?? HttpContext.GetOwinContext().Get<NewsManager>();
            }
            private set
            {
                _newsManager = value;
            }
        }

        public ClanEventManager ClanEventManager
        {
            get
            {
                return _clanEventManager ?? HttpContext.GetOwinContext().Get<ClanEventManager>();
            }
            private set
            {
                _clanEventManager = value;
            }
        }

        public IProfilePictureSystem ProfilePictureSystem
        {
            get
            {
                return _profilePictureSystem ?? new ProfilePictureSystem();
            }
            set
            {
                _profilePictureSystem = value;
            }
        }


        


        public HomeController()
        {

        }



        //
        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }


        //
        // PartialView In: Home/Index - Shows the events.
        [ChildActionOnly]
        public PartialViewResult Events()
        {
            // Getting the data
            ICollection<EventListingViewModel> model = new List<EventListingViewModel>();
            IEnumerable<ClanEvent> clanEvents = ClanEventManager.GetClanEvents();

            // Order by date and take the first 6
            clanEvents = clanEvents.OrderBy(e => e.EventDateAndTime).Take(6);

            // Converting them to viewmodels
            foreach (ClanEvent clanEvent in clanEvents)
            {
                model.Add(new EventListingViewModel
                {
                    Id = clanEvent.Id,
                    EventName = clanEvent.Title,
                    EventDate = clanEvent.EventDateAndTime
                });
            }

            return PartialView("EventsAreaPartial", model);
        }


        //
        // PartialView In: Home/Index - Shows the news articles
        [ChildActionOnly]
        public PartialViewResult PreviewNewsArticles()
        {
            // Getting the data
            ICollection< NewsArticlePreviewViewModel> model = new List<NewsArticlePreviewViewModel>();
            IEnumerable<NewsArticle> newsArticles = NewsManager.GetNewsArticles();

            // Sort the news articles
            newsArticles = newsArticles.OrderBy(n => n.Date);

            // Creating the view models
            foreach (NewsArticle article in newsArticles)
            {
                model.Add(new NewsArticlePreviewViewModel
                {
                    Id = article.Id,
                    Date = article.Date,
                    TextArea = article.ShortText,
                    Title = article.Title
                });
            }

            return PartialView("NewsAreaPartial", model);
        }


        //
        // AJAX: Getting the event details
        [HttpGet]
        public async Task<JsonResult> FullEventDetails(int eventId)
        {
            ClanEvent clanEvent = await ClanEventManager.FindByIdAsync(eventId);
            
            // Creating the view model
            EventViewModel model = new EventViewModel
            {                
                EventName = clanEvent.Title,
                EventDateAndTime = clanEvent.EventDateAndTime.ToString("dd-MM-yyy HH:mm"),
                Location = clanEvent.Location,
                Game = clanEvent.Game,
                CreatedBy = clanEvent.CreatedBy.UserName,
                ImageLocation = clanEvent.ImageLocation,
                Description = clanEvent.Description
            };

            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: Home/NewsArticle/Id
        public async Task<ActionResult> NewsArticle(int id)
        {
            // Getting the news article form the database
            NewsArticle article = await NewsManager.FindByIdAsync(id);
            
            // Getting the article
            NewsArticleViewModel model = new NewsArticleViewModel
            {
                Title = article.Title,
                TextArea = article.LongText,
                Date = article.Date,
                CreatedBy = article.CreatedBy.UserName
            };

            return View(model);
        }


        //
        // GET: Home/AllNewsArticles/Page
        public async Task<ActionResult> NewsArticles(int? page)
        {
            const ushort ItemsPerPage = 30;

            IEnumerable<NewsArticle> newsArticles = await NewsManager.GetNewsArticlesAsync();
            ICollection<NewsArticlesListViewModel> model = new List<NewsArticlesListViewModel>();

            // Converting all the articles into news articles
            foreach (NewsArticle article in newsArticles)
            {
                model.Add(new NewsArticlesListViewModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    CreatedBy = article.CreatedBy.UserName,
                    Published = article.Date
                });
            }
          
            return View(model.AsEnumerable().ToPagedList(page ?? 1, ItemsPerPage));
        }


        //
        // GET: Home/About
        public ActionResult About()
        {
            ViewBag.Message="About Us";

            return View();
        }


        //
        // GET: Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message="Your contact page.";

            return View();
        }


        //
        // GET: Home/Members
        [HttpGet]
        public async Task<ActionResult> Members()
        {
            ICollection<UserCardViewModel> usersCards = new List<UserCardViewModel>();
            MembersViewModel model = new MembersViewModel();
            IEnumerable<User> users = await UserManager.Users.ToListAsync();
            IProfilePictureSystem profilePictureSystem = ProfilePictureSystem;

            // Converting everyting to the inner view model
            foreach (User user in users)
            {
                usersCards.Add(new UserCardViewModel
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    Role = user.PrimaryRole,
                    Seniority = user.Seniority,
                    Country =  user.Country,
                    Image = profilePictureSystem.GetProfilePicture(user.Id)
                });
            }

            // Filling the model
            model.Administrators = usersCards.Where(u => UserManager.IsInRole(u.Id, "Administrators"));
            model.Moderators = usersCards.Where(u => UserManager.IsInRole(u.Id, "Moderators"));
            model.Members = usersCards.Where(u => UserManager.IsInRole(u.Id, "Members"));


            return View(model);
        }





        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_clanEventManager != null)
                {
                    _clanEventManager.Dispose();
                    _clanEventManager = null;
                }

                if (_newsManager != null)
                {
                    _newsManager.Dispose();
                    _newsManager = null;
                }
            }

            base.Dispose(disposing);
        }


        #endregion

    }
}