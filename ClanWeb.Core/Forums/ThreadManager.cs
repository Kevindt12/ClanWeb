using System;
using System.Collections.Generic;
using System.Text;
using ClanWeb.Data.Repository.EntityFramework;
using ClanWeb.Data.Entities.Forum;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ClanWeb.Core.Forums
{
    public class ThreadManager : IDisposable
    {

        public DatabaseContext Context { get; set; }
        public ForumUserManager ForumUserManager { get; set; }
        public string CurrentUserId
        {
            get
            {
                return HttpContext.Current.User.Identity.GetUserId() ?? null;
            }
        }


        public ThreadManager()
        {
            Context = new DatabaseContext();
            ForumUserManager = new ForumUserManager();
        }







        /// <summary>
        /// Finds a thread by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Thread> FindByIdAsync(int id)
        {
            return await Context.Threads.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Gets all the threads from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Thread>> GetThreadAsync()
        {
            return await Context.Threads.AsNoTracking().ToListAsync();
        }



        /// <summary>
        /// Creates a new Thread
        /// </summary>
        /// <param name="thread"></param>
        /// <returns>The thread that was just created</returns>
        public async Task<Thread> CreateAsnyc(Thread thread, Group parrent)
        {
            // attach all the relations
            Context.Groups.Attach(parrent);
            Context.Threads.Add(thread);

            // Settings some default values
            thread.Group = parrent;
            thread.LastEdited = DateTime.Now;
            thread.User = await Context.ForumUsers.FindAsync(CurrentUserId);

            await Context.SaveChangesAsync();

            return thread;
        }


        /// <summary>
        /// Creates a new thread with a new message attached to it
        /// </summary>
        /// <param name="thread">The Thread that wants to be created</param>
        /// <param name="message">The first message of that thread</param>
        /// <param name="parrent">The parrent group of that thread</param>
        /// <returns>The newly created thread</returns>
        public async Task<Thread> CreateWithMessageAsync(Thread thread, Message message, Group parrent)
        {
            // Gets the current user
            ForumUser currentForumUser = await Context.ForumUsers.SingleAsync(u => u.UserId == CurrentUserId);

            // Attaching everything to the context
            Context.Threads.Add(thread);
            Context.Groups.Attach(parrent);

            // Setting the relations of the thread and the message
            thread.Group = parrent;
            thread.Messages.Add(message);
            thread.User = currentForumUser;
            message.User = currentForumUser;

            // Make sure it adds the message
            Context.Entry(message).State = EntityState.Added;

            // Settings some default values
            thread.LastEdited = DateTime.Now;
            message.TimeStamp = DateTime.Now;
            message.MessagePlacementInThread = 0;

            await Context.SaveChangesAsync();

            return thread;
        }



        /// <summary>
        /// Updates a thread
        /// </summary>
        /// <param name="thread">The thread that needs to be updated</param>
        /// <returns></returns>
        public async Task UpdateAsync(Thread thread)
        {
            // Attaching the entity to entity framework
            Context.Threads.Attach(thread);

            // Change the last edited time of the thread to now
            thread.LastEdited = DateTime.Now;

            // Saving the entry in the database
            Context.Entry(thread).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a thread
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Thread thread)
        {
            Context.Threads.Remove(thread);

            // Makes sure you also remove the messages
            Context.Messages.RemoveRange(thread.Messages);

            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a thread
        /// </summary>
        /// <param name="thread">The thread id</param>
        /// <returns></returns>
        public async Task DeleteAsync(int threadId)
        {
            await DeleteAsync(await Context.Threads.FindAsync(threadId));
        }


        /// <summary>
        /// Checks to see if the user is subscribed to a thread (It will get the current user if there is none loged it it will thorw a exeption)
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public  bool IsUserSubscribedToThreac(Thread thread)
        {
            return Context.ForumUsers.Find(CurrentUserId).Subscriptions.Contains(thread);
        }

        /// <summary>
        /// Checks to see if the user is subscribed to a thread (It will get the current user if there is none loged it it will thorw a exeption)
        /// </summary>
        /// <param name="threadid"></param>
        /// <returns></returns>
        public  bool IsUserSubscribedToThread(int threadid)
        {
            return IsUserSubscribedToThreac(Context.Threads.Find(threadid));
        }


        /// <summary>
        ///  Adds a view count to the page
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public async Task AddThreadViewwCountAsync(int threadId)
        {
            Thread thread = await Context.Threads.FindAsync(threadId);
            thread.ViewCount++;
            await Context.SaveChangesAsync();
        }


        public virtual void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            if (ForumUserManager != null)
            {
                ForumUserManager.Dispose();
                ForumUserManager = null;
            }
        }
    }
}
