using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

using ClanWeb.Web.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ClanWeb.Core.Forums;
using System.Configuration;
using ClanWeb.Web.ApplicationCode.UI.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ClanWeb.Core.Identity;
using System.Threading.Tasks;
using ClanWeb.Data.Entities.Forum;
using PagedList;
using ClanWeb.Data.Entities;

namespace ClanWeb.Web.Controllers
{
    [Authorize]
    public partial class ForumController : Controller
    {
        private UserManager _userManager;
        private ForumService _forumService;
        private ReputationSystem _reputationSystem;
        private ForumUserManager _forumUserManager;
        private GroupManager _groupManager;
        private MessageManager _messageManager;
        private PrivateMessagingSystem _privateMessagingSystem;
        private IProfilePictureSystem _profilePictureSystem;
        private ThreadManager _threadManager;
        private RoleManager _roleManager;
        private SubscriptionManager _subscriptionManager;

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

        public ForumService ForumService
        {
            get
            {
                return _forumService ?? HttpContext.GetOwinContext().Get<ForumService>();
            }
            private set
            {
                _forumService = value;
            }
        }
        public ReputationSystem ReputationSystem
        {
            get
            {
                return _reputationSystem ?? HttpContext.GetOwinContext().Get<ReputationSystem>();
            }
            private set
            {
                _reputationSystem = value;
            }
        }
        public ForumUserManager ForumUserManager
        {
            get
            {
                return _forumUserManager ?? HttpContext.GetOwinContext().Get<ForumUserManager>();
            }
            private set
            {
                _forumUserManager = value;
            }
        }
        public GroupManager GroupManager
        {
            get
            {
                return _groupManager ?? HttpContext.GetOwinContext().Get<GroupManager>();
            }
            private set
            {
                _groupManager = value;
            }
        }
        public MessageManager MessageManager
        {
            get
            {
                return _messageManager ?? HttpContext.GetOwinContext().Get<MessageManager>();
            }
            private set
            {
                _messageManager = value;
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
        public PrivateMessagingSystem PrivateMessagingSystem
        {
            get
            {
                return _privateMessagingSystem ?? HttpContext.GetOwinContext().Get<PrivateMessagingSystem>();
            }
            private set
            {
                _privateMessagingSystem = value;
            }
        }
        public ThreadManager ThreadManager
        {
            get
            {
                return _threadManager ?? HttpContext.GetOwinContext().Get<ThreadManager>();
            }
            private set
            {
                _threadManager = value;
            }
        }
        public IProfilePictureSystem ProfilePictureSystem
        {
            get
            {
                return _profilePictureSystem ?? new ProfilePictureSystem();
            }
            private set
            {
                _profilePictureSystem = value;
            }
        }

        public SubscriptionManager SubscriptionManager
        {
            get
            {
                return _subscriptionManager ?? HttpContext.GetOwinContext().Get<SubscriptionManager>();
            }
            private set
            {
                _subscriptionManager = value;
            }
        }



        /*
         * TODO: Make groupset defferent forum or popup
         * TODO: Add Delete/Edit For Groupset Groups and Threads
         * TODO: Finish the forum manager
         * TODO: Set the rules for the actionresults E,g Admin, Moderator
         */




        public ForumController()
        {

        }




        //
        // GET: Forum
        public async Task<ActionResult> Index()
        {
            // Checks if there are groups
            if (await GroupManager.AreThereAnyGroupsAsnyc())
            {
                return RedirectToAction("Groups");
            }
            else
            {
                return RedirectToAction("AddGroup");
            }

        }



        //
        // GET: Forum/AddGroup
        [Authorize(Roles = "ManageGroups")]
        public async Task<ActionResult> AddGroup()
        {
            // Populate the view bag with all the categories
            ViewBag.Categories = (await GroupManager.GetGroupsCategoriesAsync()).AsEnumerable();

            return View();
        }

        //
        // POST: Forum/AddGroup
        [HttpPost]
        [Authorize(Roles = "ManageGroups")]
        public async Task<ActionResult> AddGroup(AddGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                Group group = new Group();

                // Getting the group propoties
                group.Name = model.Name;
                group.Description = model.Description;
                group.Category = model.Category;

                // Checking if there was a image uploaded
                if (model.Image != null)
                {
                    await GroupManager.CreateAsync(group, Bitmap.FromStream(model.Image.InputStream));
                }
                else
                {
                    await GroupManager.CreateAsync(group);
                }
            }
            else
            {
                return View(model);
            }

            // Than go to the groups view
            return RedirectToAction("Groups");
        }


        //
        // GET: Forum/AddThread/GroupId
        public ActionResult AddThread(int groupId)
        {
            // Sending info to the viewbag
            ViewBag.GroupId = groupId;

            return View();

        }


        //
        // GET: Forum/Thread/ThreadId&GroupId
        public async Task<ActionResult> Thread(int threadId, int groupId, int? page)
        {
            const int messagesPerPage = 20;

            // Filling in the viewbags
            ViewBag.GroupName = await GroupManager.GetGroupNameByIdAsync(groupId);
            ViewBag.GroupId = groupId;
            ViewBag.ThreadId = threadId;

            // Creating a list of messages
            ThreadViewModel model = new ThreadViewModel();
            Thread thread = await ThreadManager.FindByIdAsync(threadId);
            ICollection<MessagesViewModel> messagesViewModels = new List<MessagesViewModel>();

            model.ThreadName = thread.Name;

            // TODO: make this so this is only if the user is loged in
            model.Subscribed = ThreadManager.IsUserSubscribedToThreac(thread);

            // Creating the messages
            foreach (Message message in thread.Messages)
            {
                messagesViewModels.Add(new MessagesViewModel
                {
                    Id = message.Id,
                    UserName = UserManager.FindById(message.User.UserId).UserName,
                    JoinDate = message.User.JoinDate,
                    Posts = message.User.PostsCount,
                    Signiture = message.User.Signature,
                    Reputation = message.Reputation,
                    ThreadId = threadId,
                    GroupId = groupId,
                    Image = ProfilePictureSystem.GetProfilePicture(message.User.UserId),
                    Country = (await UserManager.FindByIdAsync(message.User.UserId)).Country,
                    VotingEnabled = ReputationSystem.CanUserVote(message),
                    IsCreator = (message.User.UserId == User.Identity.GetUserId()),

                    Body = message.Body,
                    MessagePost = message.TimeStamp,
                    MessageNumber = message.MessagePlacementInThread
                });
            }

            // Sorts the list by date
            messagesViewModels = messagesViewModels.OrderByDescending(m => m.MessagePost).ToList();

            // Changing to it a page list
            model.Messages = messagesViewModels.ToPagedList(page ?? 1, messagesPerPage);

            // Increase the view count
            await ThreadManager.AddThreadViewwCountAsync(thread.Id);

            return View(model);
        }




        //
        // GET: /Forums/Groups
        public async Task<ActionResult> Groups()
        {
            //ICollection<GroupSetViewModel> model = new List<GroupSetViewModel>();
            IDictionary<string, ICollection<GroupViewModel>> model = new Dictionary<string, ICollection<GroupViewModel>>();
            IEnumerable<Group> groups = await GroupManager.GetGroupsAsync();


            // Getting the group sets
            foreach (Group group in groups)
            {
                int groupId = group.Id;

                GroupViewModel subModel = new GroupViewModel
                {
                    Id = groupId,
                    Name = group.Name,
                    Description = group.Description,
                    Image = group.Image ?? "string",                  // TODO: This needs to be checked if its null there has to be a defualt image 
                    ThreadCount = group.Threads.Any() ? group.Threads.Count() : 0,
                    PostCount = await GroupManager.GetPostsCount(groupId)
                };

                // Check if there are any threads
                if (group.Threads.Any())
                {
                    Thread lastThread = GroupManager.GetLastActiveThreadByGroupIdAsync(groupId);
                    subModel.LastPostedThreadName = lastThread.Name;
                    subModel.LastPostedThreadUserName = lastThread.User.UserName;
                }
                else
                {
                    subModel.LastPostedThreadName = "None";
                    subModel.LastPostedThreadUserName = "None";
                }

                // Seperating based on the category
                if (model.ContainsKey(group.Category))
                {
                    model[group.Category].Add(subModel);
                }
                else
                {
                    model.Add(group.Category, new List<GroupViewModel>());
                    model[group.Category].Add(subModel);
                }
            }

            return View(model);
        }


        //
        // GET: Forum/Threads/GroupId
        public async Task<ActionResult> Threads(int groupId, int? page)
        {
            ICollection<ThreadsViewModel> model = new List<ThreadsViewModel>();
            Group group = await GroupManager.FindByIdAsync(groupId);
            IEnumerable<Thread> threads = group.Threads;

            const int threadsPerPage = 40;

            ViewBag.GroupId = groupId;
            ViewBag.GroupName = await GroupManager.GetGroupNameByIdAsync(groupId);

            // Sorting first on date and then on if there pinned
            threads = threads.OrderByDescending(t => t.Messages.LastOrDefault().TimeStamp).OrderBy(t => t.Pinned).AsEnumerable();

            foreach (Thread thread in threads)
            {
                model.Add(new ThreadsViewModel
                {
                    Id = thread.Id,
                    Name = thread.Name,
                    LastMessageTimeStamp = thread.Messages.LastOrDefault().TimeStamp,
                    LastMessageUserName = thread.Messages.LastOrDefault().User.UserName,
                    NumberOfMessages = thread.Messages.Count(),
                    StartedByUserName = thread.User.UserName,
                    StartedTimeStamp = thread.Messages.FirstOrDefault().TimeStamp.ToLongTimeString(),
                    ViewCount = thread.ViewCount,
                    Vote = await ReputationSystem.GetThreadReputationAsync(thread)
                });
            }

            return View(model.ToPagedList(page ?? 1, threadsPerPage));
        }




        // POST: Forum/AddThread/GroupId
        [HttpPost]
        public async Task<ActionResult> AddThread(int groupId, AddThreadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ViewBag.GroupId = groupId;

            // Creating the thread
            Thread thread = new Thread
            {
                Name = model.Name,
                Tags = (!String.IsNullOrEmpty(model.Tags)) ? model.Tags.Split(',').ToList().Where(s => !String.IsNullOrEmpty(s)).Select(s => s.Trim()).ToArray() : new string[0],
                Pinned = model.Pinned
            };

            // Creating the message (There not connected here becouse they will be in the core of the code)
            Message message = new Message
            {
                Body = model.FirstMessage
            };

            // Save the new thread and returns the result
            thread = await ThreadManager.CreateWithMessageAsync(thread, message, await GroupManager.FindByIdAsync(groupId));

            return RedirectToAction("Groups");
        }


        //
        // POST: Forum/Thread | This is a partial Retrun
        [HttpPost]
        public async Task<ActionResult> AddMessagePost(AddMessageViewModel model, int threadId, int groupId)
        {
            // Checking if the model is valled
            if (ModelState.IsValid)
            {
                ViewBag.ThreadId = threadId;
                ViewBag.GroupId = groupId;

                // Creating the message
                Message message = new Message
                {
                    Body = model.Body
                };

                await MessageManager.CreateAsync(message, await ThreadManager.FindByIdAsync(threadId));
            }
            else
            {
                return Content("There was a error with your message please check before you post again");
            }

            return RedirectToAction("Thread", new { threadId, groupId });
        }


        //
        // GET: Forum/EditGroup?groupid
        [Authorize(Roles = "ManageGroups")]
        public async Task<ActionResult> EditGroup(int groupId)
        {
            ViewBag.GroupId = groupId;

            // Getting the group
            Group group = await GroupManager.FindByIdAsync(groupId);

            // Convert and fill the model
            EditGroupViewModel model = new EditGroupViewModel
            {
                Name = group.Name,
                Description = group.Description,
                Category = group.Category,
                ImageUrl = group.Image
            };

            // Gets all the group sets
            ViewBag.GroupSetList = await GroupManager.GetGroupsCategoriesAsync();

            return View(model);
        }

        //
        // POST: Forum/EditGroup?groupid
        [HttpPost]
        [Authorize(Roles = "ManageGroups")]
        public async Task<ActionResult> EditGroup(EditGroupViewModel model, int groupId)
        {
            ViewBag.GroupId = groupId;

            Group group = await GroupManager.FindByIdAsync(groupId);

            // Getting the group propoties
            group.Name = model.Name;
            group.Description = model.Description;
            group.Category = model.Category;

            // Checking if there was a image uploaded
            if (model.Image != null)
            {
                await GroupManager.UpdateAsync(group, Bitmap.FromStream(model.Image.InputStream));
            }
            else
            {
                await GroupManager.UpdateAsync(group);
            }

            return RedirectToAction("Groups");
        }


        public async Task<ActionResult> EditThread(int groupId, int threadId)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ThreadId = threadId;

            // Getting the thread
            Thread thread = await ThreadManager.FindByIdAsync(threadId);
            EditThreadViewModel model = new EditThreadViewModel
            {
                Name = thread.Name,
                Tags = String.Join(", ", thread.Tags)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditThread(int groupId, int threadId, EditThreadViewModel model)
        {
            // Gets the thread
            Thread thread = await ThreadManager.FindByIdAsync(threadId);

            // Changes the values
            thread.Tags = model.Tags.Split(',').ToList().Select(s => s.Trim()).Where(s => !String.IsNullOrEmpty(s)).ToArray();
            thread.Name = model.Name;

            // Updates the thread
            await ThreadManager.UpdateAsync(thread);

            return RedirectToAction("Thread");
        }


        [HttpPost]
        public async Task<JsonResult> AddThreadToSubscriptions(int threadId)
        {

            await SubscriptionManager.RemoveThreadToSubscriptionsAsync(threadId);

            return Json(new { subscribed = true });
        }


        [HttpPost]
        public async Task<JsonResult> RemoveThreadFronSubscriptions(int threadId)
        {

            // Remove this thread to the users subscription
            await SubscriptionManager.RemoveThreadToSubscriptionsAsync(threadId);


            return Json(new { subscribed = false });
        }


        //
        // POST: Forum/RemoveGroup/GroupId
        [Authorize(Roles = "ManageGroups")]
        public async Task<ActionResult> RemoveGroup(int groupId)
        {
            ViewBag.GorupId = groupId;

            // Checking if the groupsets are still up to date
            await GroupManager.DeleteAsync(groupId);

            // Returns to the group view
            return RedirectToAction("Groups");
        }


        //
        // POST: Forum/RemoveThread/Groupid&ThreadId
        public async Task<ActionResult> RemoveThread(int groupId, int threadId)
        {
            string currentUserId = User.Identity.GetUserId();
            Thread thread = await ThreadManager.FindByIdAsync(threadId);


            // Check if user can remove thread
            if (UserManager.IsInRole(currentUserId, "ThreadModerator") || thread.User.User.Id == currentUserId)
            {

                ViewBag.GroupId = groupId;
                ViewBag.ThreadId = threadId;

                await ThreadManager.DeleteAsync(threadId);

                // Go back to groups
                return RedirectToAction("Threads", new { groupId = groupId });
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }


        //
        // POST: Forum/RemoveMessage | This does not have the post attribute Because it does not work for now
        // But this is just a return so it will change the database data. There is already protection agenst acsedents
        public async Task<ActionResult> RemoveMessage(int groupId, int threadId, int messageId)
        {
            string currentUserId = User.Identity.GetUserId();
            Message message = await MessageManager.FindByIdAsync(messageId);

            // Check if user can remove thread
            if (UserManager.IsInRole(currentUserId, "ThreadModerator") || message.User.User.Id == currentUserId)
            {

                ViewBag.GroupId = groupId;
                ViewBag.ThreadId = threadId;
                ViewBag.MessageId = messageId;

                await MessageManager.DeleteAsync(messageId);

                // Reload the thread
                return RedirectToAction("Thread", new { groupId = groupId, threadId = threadId });
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }


        //
        // GET: Forum/EditMessage/Groupid&ThreadId&MessageId
        public async Task<ActionResult> EditMessage(int groupId, int threadId, int messageId)
        {
            // Just sending the id too the viewbag so if we need them in the view we have them
            ViewBag.GroupId = groupId;
            ViewBag.ThreadId = threadId;
            ViewBag.MessageId = messageId;

            Message message = await MessageManager.FindByIdAsync(messageId);

            // The model for the orgirinal message
            // Creating the block
            MessagesViewModel messageModel = new MessagesViewModel
            {
                Id = message.Id,
                UserName = message.User.UserName,
                JoinDate = message.User.JoinDate,
                Posts = message.User.PostsCount,
                Signiture = message.User.Signature,
                Reputation = message.Reputation,
                ThreadId = threadId,
                GroupId = groupId,
                Image = ProfilePictureSystem.GetProfilePicture(message.User.UserId),
                Country = (await UserManager.FindByIdAsync(message.User.UserId)).Country,
                Body = message.Body,
                MessagePost = message.TimeStamp,
                MessageNumber = message.MessagePlacementInThread
            };

            // This will be passed true to the partial view
            ViewBag.MessageModel = messageModel;

            return View();
        }

        //
        // POST: Forum/EditMessage/Groupid&ThreadId&MessageId
        [HttpPost]
        public async Task<ActionResult> EditMessage(int groupId, int threadId, int messageId, EditMessageViewModel model)
        {
            ViewBag.GroupId = groupId;
            ViewBag.ThreadId = threadId;
            ViewBag.MessageId = messageId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get the message we want to modifile
            Message message = await MessageManager.FindByIdAsync(messageId);

            // Chaning the properties
            message.Body = model.Body;

            // Updating that change
            await MessageManager.UpdateAsync(message);

            // Go back to the tread
            return RedirectToAction("Thread", new { groupId = groupId, threadId = threadId });
        }






        //
        // GET: Forum\ForumProfile
        public async Task<ActionResult> ForumProfile(string userName)
        {

            ForumUser user = await ForumUserManager.FindByUserNameAsync(userName);

            ForumProfleViewModel model = new ForumProfleViewModel
            {
                UserName = user.UserName,
                ProfilePicture = ProfilePictureSystem.GetProfilePicture(user.UserId),
                PostCount = user.PostsCount,
                Reputation = user.Reputation,
                BaseRole = user.User.PrimaryRole
            };

            return View(model);
        }


        //
        // GET: Forum\LastActiverty
        public async Task<ActionResult> LastActiverty(int? page)
        {
            string userId = User.Identity.GetUserId();
            const int itemsPerPage = 20;
            ICollection<LastActuvityThreadsViewModel> model = new List<LastActuvityThreadsViewModel>();
            IEnumerable<Message> postedMessageOfUser = (await ForumUserManager.FindByIdAsync(userId)).PostedMessages;


            // Getting each message That the user has posted and checking if the message is older than 3 monthes
            foreach (Message message in postedMessageOfUser.Where(m => m.TimeStamp > DateTime.Now.AddMonths(-3)))
            {
                model.Add(new LastActuvityThreadsViewModel
                {
                    TheadName = message.Thread.Name,
                    DateOfActivery = message.TimeStamp,
                    GroupId = message.Thread.Group.Id,
                    ThreadId = message.Thread.Id
                });
            }

            // Ordering the models by date
            model = model.OrderBy(x => x.DateOfActivery).ToList();

            return View(model.ToPagedList(page ?? 1, itemsPerPage));
        }


        //
        // GET: Forum/Subscriptions
        public async Task<ActionResult> Subscriptions(int? page)
        {
            const int itemsPerPage = 20;
            ICollection<SubscriptionsThreadsViewModel> model = new List<SubscriptionsThreadsViewModel>();
            IEnumerable<Thread> SubscribedTo = await SubscriptionManager.GetSubscriptions();

            // Getting all the threads that the user is subscribed to
            foreach (Thread thread in SubscribedTo)
            {
                model.Add(new SubscriptionsThreadsViewModel
                {
                    ThreadName = thread.Name,
                    ThreadId = thread.Id,
                    GroupId = thread.Group.Id,
                    LastUserThatAddedAMessage = thread.Messages.LastOrDefault().User.UserName,
                    LastEditedToThread = thread.Messages.LastOrDefault().TimeStamp
                });
            }

            return View(model.ToPagedList(page ?? 1, itemsPerPage));
        }

        public async Task<ActionResult> UserProfile(string username)
        {
            // Get the user
            ForumUser forumUser = await ForumUserManager.FindByUserNameAsync(username);
            User user = forumUser.User;

            // Populate the model
            UserProfileViewModel model = new UserProfileViewModel()
            {
                UserName = user.UserName,
                Reputation = forumUser.Reputation,
                Postcount = forumUser.PostsCount,
                Role = user.PrimaryRole,
                LastActivity = forumUser.LastActivity,
                JoinDate = forumUser.JoinDate,
                Country = user.Country,
                Seniorty = user.Seniority,
                Image = ProfilePictureSystem.GetProfilePicture(user.Id),
            };

            return View(model);
        }

        // TODO: Make it so you cant vote on your own message
        // AJAX: A ajax post for reputation buttons Add
        [HttpPost]
        public async Task<JsonResult> MessageReputationUp(int messageId)
        {
            // We will upvote the mssage and get the current reputation on the message
            int messageReputation = await ReputationSystem.UpVoteOnMessageAsync(messageId);

            // we will return the current reputation
            return Json(new { reputation = messageReputation });
        }

        //
        // AJAX: A ajax post for reputation buttons Subtract
        [HttpPost]
        public async Task<JsonResult> MessageReputationDown(int messageId)
        {
            // We will downvote on the message and get the current repuitation on the message
            int messageReputation = await ReputationSystem.DownVoteOnMessageAsync(messageId);

            // we will return that current reputation
            return Json(new { reputation = messageReputation });

        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_forumService != null)
                {
                    _forumService.Dispose();
                    _forumService = null;
                }
                if (_reputationSystem != null)
                {
                    _reputationSystem.Dispose();
                    _reputationSystem = null;
                }
                if (_forumUserManager != null)
                {
                    _forumUserManager.Dispose();
                    _forumUserManager = null;
                }
                if (_groupManager != null)
                {
                    _groupManager.Dispose();
                    _groupManager = null;
                }
                if (_messageManager != null)
                {
                    _messageManager.Dispose();
                    _messageManager = null;
                }
                if (_privateMessagingSystem != null)
                {
                    _privateMessagingSystem.Dispose();
                    _privateMessagingSystem = null;
                }
                if (_threadManager != null)
                {
                    _threadManager.Dispose();
                    _threadManager = null;
                }
            }
            base.Dispose(disposing);
        }


        #region Helpers



        #endregion




    }
}