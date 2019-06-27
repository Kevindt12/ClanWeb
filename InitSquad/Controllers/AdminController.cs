using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using ClanWeb.Web.Models;
using ClanWeb.Core.Identity;

using System.Drawing;
using ClanWeb.Core.Infomation;
using ClanWeb.Data.Entities;
using System.Data.Entity;
using PagedList;
using ClanWeb.Core.Comunication;

namespace ClanWeb.Web.Controllers
{

    [Authorize(Roles = "Administrators,Moderators")]
    public class AdminController : Controller
    {
        private UserManager _userManager;
        public RoleManager _roleManager;
        private NewsManager _newsManager;
        private ClanEventManager _eventManager;
        private IEmailService _emailService;

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

        public ClanEventManager EventManager
        {
            get
            {
                return _eventManager ?? HttpContext.GetOwinContext().Get<ClanEventManager>();
            }
            private set
            {
                _eventManager = value;
            }
        }

        public IEmailService EmailService
        {
            get
            {
                return _emailService ?? HttpContext.GetOwinContext().Get<IEmailService>();

            }
            set
            {
                _emailService = value;

            }
        }



        public AdminController()
        {

        }



        //
        // Get: /Admin/Index        
        public ActionResult Index()
        {
            ViewBag.ActivePage = "Index";
            return View();
        }


        //
        // GET: Admin/Users
        [Authorize(Roles= "SeeUsers")]
        public async Task<ActionResult> Users(int? page)
        {
            ViewBag.ActivePage = "Users";

            const int UsersPerPage = 10;           
            IEnumerable<User> users = await UserManager.Users.ToListAsync();
            ICollection<UsersViewModel> model = new List<UsersViewModel>();

            foreach (User user in users)
            {
                model.Add(new UsersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Seniority = user.Seniority,
                    Role = user.PrimaryRole
                });
            }

            return View(model.AsEnumerable().ToPagedList(page ?? 1, UsersPerPage));
        }

        [HttpPost]
        [Authorize(Roles = "ApproveUsers")]
        public async Task<JsonResult> ApproveUser(string userId)
        {
            // Approve the user
            await UserManager.ApproveUserAsync(userId);

            // Notifying the user that we have activated there account
            await EmailService.SendAsync(new IdentityMessage
            {
                Destination = await UserManager.GetEmailAsync(userId),
                Subject = "REPLACECLANNAME Your account has been activated",
                Body = "Welcome to the clan. Your account has been activated you can now join the forums and make full use of the discord"
            });

            // Returning true iondicating that the apprval has been success
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize(Roles = "ApproveUsers")]
        public async Task<JsonResult> DispproveUser(string userId)
        {
            // Approve the user
            User user = await UserManager.FindByIdAsync(userId);
            await UserManager.DisapproveUserAsync(userId);

            // Notifying the user that we have activated there account
            await EmailService.SendAsync(new IdentityMessage
            {
                Destination = await UserManager.GetEmailAsync(userId),
                Subject = "REPLACECLANNAME Your account has been denaid",
                Body = "Your approval for the clan has been denaild sorry for the inconvinance. Here by we have also delete your from our servers. If your on our discord you wont be removed from it."
            });

            // Returning true iondicating that the apprval has been success
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        //
        // GET: /Admin/EditUserRole
        [Authorize(Roles = "EditUserRole")]
        public async Task<ActionResult> EditUserRole(string userId)
        {
            ViewBag.ActivePage = "Users";

            ChangeRoleViewModel model = new ChangeRoleViewModel();
            User user = await UserManager.FindByIdAsync(userId);
            IList<string> primaryRoles = new string[] { "Administrators", "Moderators", "Members" };
            IList<string> roleNames = RoleManager.Roles.ToList().Select((r) => r.Name).ToList();
            IEnumerable<string> userRoles = (await UserManager.GetRolesAsync(userId)).AsEnumerable();

            // Remove the primary roles form the rle manes
            primaryRoles.ToList().ForEach(p => roleNames.Remove(p));

            // Filling the advanced roles by checking for each if the user is in that role
            foreach (string role in roleNames)
            {
                // Creating and setting the base values of the block
                model.Roles.Add(new Role
                {
                    Name = role,
                    Checked = userRoles.Contains(role)
                });
            }

            ViewBag.UserId = userId;

            // Fills in the view model
            model.PrimaryRole = user.PrimaryRole;

            // Creating the primary role select list items
            IEnumerable<SelectListItem> PrimaryRolesSelectList = new SelectList(primaryRoles, userRoles.ToList().Where(r => primaryRoles.Contains(r)));
            ViewBag.PrimaryRolesSelectList = PrimaryRolesSelectList;

            return View(model);
        }

        //
        // POST: /Admin/EditUserRole
        [HttpPost]
        [Authorize(Roles = "EditUserRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserRole(string userId, ChangeRoleViewModel model)
        {
            ViewBag.ActivePage = "Users";

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Users");
            }

            User user = await UserManager.FindByIdAsync(userId);

            // Remove all roles
            await UserManager.RemoveFromRolesAsync(user.Id, UserManager.GetRoles(user.Id).ToArray());

            // Adding the roles that where selected
            model.Roles.Where((r) => r.Checked).Select(r => r.Name).ToList().ForEach(r => UserManager.AddToRole(user.Id, r));

            // And add the primary role
            await UserManager.AddToRoleAsync(user.Id, model.PrimaryRole);

            return RedirectToAction("Users");
        }


        //
        // GET: /Admin/EditUser      
        [Authorize(Roles = "EditUsers")]
        public async Task<ActionResult> EditUser(string id)
        {
            ViewBag.ActivePage = "Users";

            // Finds the user
            User user = await UserManager.FindByIdAsync(id);

            // Gets the data from the user and returns that
            EditUserViewModel model = new EditUserViewModel
            {
                UserId = user.Id,
                Seniority = user.Seniority
            };

            return View(model);
        }

        //
        // POST: /Admin/EditUser
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EditUser")]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            ViewBag.ActivePage = "Users";

            // Checking if the form was filled in propoly
            if (ModelState.IsValid)
            {
                // Gets the user
                User user = await UserManager.FindByIdAsync(model.UserId);

                // Editing the user || Add more thing when model expands
                user.Seniority = model.Seniority;

                // Save the user data
                await UserManager.UpdateAsync(user);
            }

            return View("Users");
        }


        //
        // GET: /Admin/DeleteUser
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "DeleteUsers")]
        public async Task<ActionResult> DeleteUser(string id)
        {

            // Getting the user from the database
            User user = await UserManager.FindByIdAsync(id);

            // Remove user from UserStore
            await UserManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }





        //
        // GET: Admin/NewsArticles
        [Authorize(Roles = "SeeNewsArticles")]
        public async Task<ActionResult> NewsArticles(int? page)
        {
            ViewBag.ActivePage = "NewsArticles";

            ICollection<ANewsArticlesViewModel> model = new List<ANewsArticlesViewModel>();
            const int newsArticlesPerPage = 10;

            // Going true each News article
            foreach (NewsArticle article in await NewsManager.GetNewsArticlesAsync())
            {

                model.Add(new ANewsArticlesViewModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    Date = article.Date
                });
            }

            return View(model.AsEnumerable().ToPagedList(page ?? 1, newsArticlesPerPage));
        }


        //
        // GET: /Admin/CreateNewsArticle
        [Authorize(Roles = "CreateNewsArticles")]
        public ActionResult CreateNewsArticle()
        {
            ViewBag.ActivePage = "NewsArticles";

            return View();
        }

        //
        // POST: /Admin/CreateNewsArticle
        [HttpPost]
        [Authorize(Roles = "CreateNewsArticles")]
        public async Task<ActionResult> CreateNewsArticle(CreateNewsArticleViewModel model)
        {
            // Checking if there is a model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Creating a new instance of the model
            NewsArticle article = new NewsArticle
            {
                Title = model.Title,
                LongText = model.LongText,
                Date = model.Date,
            };

            // Saving it in the database
            await NewsManager.CreateNewsArticleAsync(article);

            return RedirectToAction("NewsArticles");

        }


        //
        // GET: Admin/EditNewsArticle
        [Authorize(Roles = "EditNewsArticles")]
        public async Task<ActionResult> EditNewsArticle(int id)
        {
            ViewBag.ActivePage = "NewsArticles";

            // Finds the news article
            NewsArticle newsArticle = await NewsManager.FindByIdAsync(id);

            // Get the info it needs
            EditNewsArticleViewModel model = new EditNewsArticleViewModel
            {
                Title = newsArticle.Title,
                LongText = newsArticle.LongText,
                Date = newsArticle.Date
            };

            return View(model);
        }

        // TODO: This is a bit transacutaly heavy test this and make sure i should not refactor this
        // POST: Admin/EditNewsArticle
        [HttpPost]
        [Authorize(Roles = "EditNewsArticles")]
        public async Task<ActionResult> EditNewsArticle(int id, EditNewsArticleViewModel model)
        {
            // Checking if the model is valid
            if (ModelState.IsValid)
            {
                return View(model);
            }

            // Get the info it needs
            NewsArticle editedNewsArticle = await NewsManager.FindByIdAsync(id);

            // Change the properties that have to be edited
            editedNewsArticle.Title = model.Title;
            editedNewsArticle.LongText = model.LongText;

            // Updates the entry in the database
            await NewsManager.UpdateNewsArticleAsync(editedNewsArticle);

            // Go back to the list
            return RedirectToAction("NewsArticles");
        }


        //
        // GET: Admin/DeleteNewsArticle
        [Authorize(Roles = "DeleteNewsArticles")]
        public async Task<ActionResult> DeleteNewsArticle(int id)
        {
            // Gets the news article
            NewsArticle toDelete = await NewsManager.FindByIdAsync(id);

            // Deletes the news article
            await NewsManager.DeleteNewsArticleAsync(toDelete);

            return RedirectToAction("NewsArticles");
        }


        //
        // GET: Admin/Events
        [Authorize(Roles = "SeeEvents")]
        public async Task<ActionResult> Events(int? page)
        {
            ViewBag.ActivePage = "Events";

            const ushort eventsPerPage = 20;
            ICollection<AEventsViewModel> model = new List<AEventsViewModel>();
            IEnumerable<ClanEvent> events = await EventManager.GetClanEventsAsync();

            foreach (ClanEvent clanEvent in events)
            {
                model.Add(new AEventsViewModel
                {
                    Id = clanEvent.Id,
                    Title = clanEvent.Title,
                    Date = clanEvent.EventDateAndTime
                });
            }

            return View(model.AsEnumerable().ToPagedList(page ?? 1, eventsPerPage));
        }


        //
        // GET: /Admin/CreateEvent
        [Authorize(Roles = "CreateEvents")]
        public ActionResult CreateEvent()
        {
            ViewBag.ActivePage = "Events";

            return View(new CreateEventViewModel());
        }

        //
        // POST: /Admin/CreateEvent
        [HttpPost]
        [Authorize(Roles = "CreateEvents")]
        public async Task<ActionResult> CreateEvent(CreateEventViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Converting
            ClanEvent clanEvent = new ClanEvent
            {
                Title = model.EventName,
                EventDateAndTime = model.EventDate,
                Game = model.Game,
                Description = model.Description,
                Location = model.Location,
            };

            // This will choose if i want t osave the image also
            if (model.Image == null)
            {
                // Adding the event to the database
                await EventManager.CreateClanEventAsync(clanEvent);
            }
            else
            {
                // Adding the event to the database
                await EventManager.CreateClanEventAsync(clanEvent, Bitmap.FromStream(model.Image.InputStream));
            }

            return RedirectToAction("Events");
        }


        //
        // GET: Admin/EditEvent
        [Authorize(Roles = "EditEvents")]
        public async Task<ActionResult> EditEvent(int id)
        {
            ViewBag.ActivePage = "Events";

            ClanEvent clanEvent = await EventManager.FindByIdAsync(id);

            // Creating the event view model
            EditEventViewModel model = new EditEventViewModel
            {
                EventName = clanEvent.Title,
                EventDate = clanEvent.EventDateAndTime,
                Description = clanEvent.Description,
                Game = clanEvent.Game,
                Location = clanEvent.Location,
                OrginalImageUrl = clanEvent.ImageLocation
            };

            return View(model);
        }


        //
        // POST: Admin/EditEvent
        [HttpPost]
        [Authorize(Roles = "EditEvents")]
        public async Task<ActionResult> EditEvent(int id, EditEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Converting the block
                ClanEvent clanEvent = new ClanEvent
                {
                    Id = id,
                    Title = model.EventName,
                    EventDateAndTime = model.EventDate,
                    Description = model.Description,
                    Game = model.Game,
                    Location = model.Location,
                };

                // Saving the model based 0n if it has a image
                if (model.Image == null)
                {
                    await EventManager.UpdateClanEventAsync(clanEvent);
                }
                else
                {
                    await EventManager.UpdateClanEventAsync(clanEvent, Bitmap.FromStream(model.Image.InputStream));
                }

                return RedirectToAction("Events");
            }

            ModelState.AddModelError("Model", "You did not fill in everything");
            return View(model);
        }

        //
        // GET: Admin/DeleteEvent
        [Authorize(Roles = "DeleteEvents")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            // Deleting the model from the database
            await EventManager.DeleteClanEventAsync(await EventManager.FindByIdAsync(id));

            return RedirectToAction("Events");
        }


        //
        // GET: Admin/Approval
        [Authorize(Roles = "ApproveUsers")]
        public ActionResult Approval()
        {
            ViewBag.ActivePage = "Approval";

            ICollection<ApprovalUsersListViewModel> model = new List<ApprovalUsersListViewModel>();
            IEnumerable<User> users = UserManager.Users.Where(u => u.IsInReview == true);

            // Converting the users to the view model
            foreach (User user in users)
            {
                model.Add(new ApprovalUsersListViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            return View(model);
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    UserManager = null;
                }

                if (RoleManager != null)
                {
                    RoleManager.Dispose();
                    RoleManager = null;
                }

                if (NewsManager != null)
                {
                    NewsManager.Dispose();
                    NewsManager = null;
                }

                if (EventManager != null)
                {
                    EventManager.Dispose();
                    EventManager = null;
                }
            }
            base.Dispose(disposing);



        }





    }











}
