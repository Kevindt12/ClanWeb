using ClanWeb.Data.Entities.Forum;
using ClanWeb.Data.Repository.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClanWeb.Core.Forums
{
    public class SubscriptionManager : IDisposable
    {

        public SubscriptionManager()
        {



        }


        /// <summary>
        /// Gets all the subscriptions for the current loged in user
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Thread>> GetSubscriptions()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return (await context.ForumUsers.FindAsync(HttpContext.Current.User.Identity.GetUserId())).Subscriptions.AsEnumerable();
            }

        }

        /// <summary>
        /// Gets all the subscrtiptions for the specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Thread>> GetSubscriptions(ForumUser user)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return (await context.ForumUsers.FindAsync(user.UserId)).Subscriptions.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets all the subscrtiptions for the specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Thread>> GetSubscriptions(string userId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return (await context.ForumUsers.FindAsync(userId)).Subscriptions.AsEnumerable();
            }
        }


        /// <summary>
        /// Adds a thread to the users subscriptions
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public async Task AddThreadToSubscriptionsAsync(Thread thread)
        {
            await AddThreadToSubscriptionsAsync(thread.Id);

        }

        /// <summary>
        /// Adds a thread to the users subscriptions
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public async Task AddThreadToSubscriptionsAsync(int threadId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                // Get the user
                ForumUser user = await context.ForumUsers.FindAsync(HttpContext.Current.User.Identity.GetUserId());
                Thread thread = await context.Threads.FindAsync(threadId);

                // Adding the thread to the subscriptions of the person
                user.Subscriptions.Add(thread);

                // Adding the user  to the subscriptions of the thread
                thread.Subscribers.Add(user);

                // Saving everything to the database
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes a thread from the users subscription
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public async Task RemoveThreadToSubscriptionsAsync(Thread thread)
        {
            await RemoveThreadToSubscriptionsAsync(thread.Id);

        }


        /// <summary>
        /// Removes a thread from the users subscription
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public async Task RemoveThreadToSubscriptionsAsync(int threadId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                // Get the user
                ForumUser user = await context.ForumUsers.FindAsync(HttpContext.Current.User.Identity.GetUserId());
                Thread thread = await context.Threads.FindAsync(threadId);

                // Adding the thread to the subscriptions of the person
                user.Subscriptions.Remove(thread);

                // Adding the user  to the subscriptions of the thread
                thread.Subscribers.Remove(user);

                // Saving everything to the database
                await context.SaveChangesAsync();
            }

        }

        public virtual void Dispose()
        {
           
        }
    }
}
