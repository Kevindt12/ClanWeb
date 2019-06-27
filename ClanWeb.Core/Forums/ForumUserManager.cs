using ClanWeb.Data.Entities;
using ClanWeb.Data.Entities.Forum;
using ClanWeb.Data.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ClanWeb.Core.Forums
{
    public class ForumUserManager : IDisposable
    {


        public DatabaseContext Context { get; set; }

        public ForumUserManager()
        {
            Context = new DatabaseContext();
        }



        /// <summary>
        /// Creates a new forum user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ForumUser> CreateAsync(string userId)
        {
            // Get the user
            User user = await Context.Users.SingleAsync(u => u.Id == userId);

            ForumUser result = new ForumUser
            {
                UserId = user.Id,
                UserName = user.UserName,
                Reputation = 0,
                PostsCount = 0,
                Signature = "",
                JoinDate = DateTime.Now,
                LastActivity = DateTime.Now,
                User = user
            };

            Context.ForumUsers.Add(result);
            await Context.SaveChangesAsync();


            return result;
        }



        /// <summary>
        /// Creates a new forum user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ForumUser Create(User user)
        {
            ForumUser result = new ForumUser
            {
                UserId = user.Id,
                UserName = user.UserName,
                Reputation = 0,
                PostsCount = 0,
                Signature = "",
                JoinDate = DateTime.Now,
                LastActivity = DateTime.Now
            };

            using (DatabaseContext context = new DatabaseContext())
            {
                context.Users.Attach(user);
                context.ForumUsers.Add(result);
                result.User = user;
                context.SaveChanges();
            }

            return result;
        }


        /// <summary>
        /// Checks if a user does exist for the forums.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>Bool indicating if the user does exist</returns>
        public async Task<bool> ExistsAsync(string id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (await context.ForumUsers.AnyAsync(u => u.UserId == id))
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Checks if a user does exist for the forums.
        /// </summary>
        /// <param name="user">The base user</param>
        /// <returns>A bool indicating if the user has a forum account</returns>
        public bool Exists(User user)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (context.ForumUsers.Any(u => u.UserId == user.Id))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Finds a user by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ForumUser> FindByIdAsync(string id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.ForumUsers.FindAsync(id);
            }
        }


        /// <summary>
        /// Finds a user by the given user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ForumUser> FindByUserNameAsync(string userName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.ForumUsers.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Gets all the users in the applciation
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ForumUser>> GetUsersAsync()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.ForumUsers.ToListAsync();
            }
        }


        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ForumUser user)
        {
            if (String.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentNullException(nameof(user));
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                context.ForumUsers.Add(user);
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Removes a user from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ForumUser user)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.ForumUsers.Remove(user);
                await context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        public static async Task<ForumUser> GetCurrentUserAsync()
        {
            using (ForumUserManager forumUserManager = new ForumUserManager())
            {
                return await forumUserManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            }
        }


        public virtual void Dispose()
        {

            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}
