using ClanWeb.Data.Entities.Forum;
using ClanWeb.Data.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace ClanWeb.Core.Forums
{
    public class GroupManager : IDisposable
    {

        public DatabaseContext Context { get; set; }



        public GroupManager()
        {
            Context = new DatabaseContext();
        }



        /// <summary>
        /// Finds a thread by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Group> FindByIdAsync(int id)
        {
            return await Context.Groups.AsNoTracking().Include(g => g.Threads.Select(t => t.User)).SingleOrDefaultAsync(g => g.Id == id);
        }


        /// <summary>
        /// Gets all the threads from the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await Context.Groups.AsNoTracking().Include(g => g.Threads.Select(t => t.User)).ToListAsync();
        }


        /// <summary>
        /// Creates a new Thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task CreateAsync(Group group)
        {
            Context.Groups.Add(group);
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Creates a new Thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task CreateAsync(Group group, Image image)
        {
            Context.Groups.Add(group);
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Updates a exsisting thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Group group)
        {
            Context.Groups.Attach(group);
            Context.Entry(group).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Updates a exsisting thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Group group, Image image)
        {
            Context.Groups.Attach(group);
            Context.Entry(group).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Group group)
        {
            // Deleting the group (Because we might not have cascading delete allways on or might turn it off. We will delete the relation manualy)
            Context.Groups.Remove(group);

            // Delete aslo all the threads and the messages
            Context.Threads.RemoveRange(group.Threads);

            // Removes all the message from each thread
            group.Threads.ToList().ForEach(t => Context.Messages.RemoveRange(t.Messages));

            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a thread
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int groupId)
        {
            Group group = await Context.Groups.FindAsync(groupId);

            Context.Groups.Remove(group);

            // Delete aslo all the threads and the messages
            Context.Threads.RemoveRange(group.Threads);

            // Removes all the message from each thread
            group.Threads.ToList().ForEach(t => Context.Messages.RemoveRange(t.Messages));

            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Checks if there are any groups in the application
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AreThereAnyGroupsAsnyc()
        {
            return await Context.Groups.AnyAsync();
        }


        /// <summary>
        /// Gets the number of posts in the group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<int> GetPostsCount(int groupId)
        {
            int postCount = 0;

            Group group = await Context.Groups.FindAsync(groupId);

            // Checking if there are any threads
            if (group.Threads.Any())
            {
                // Calculates all the post counts of each group
                await Task.Run(() => group.Threads.ToList().ForEach(t => postCount += t.Messages.Count()));
            }

            return postCount;
        }


        /// <summary>
        /// Gets the name of the group by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetGroupNameByIdAsync(int id)
        {
            return (await Context.Groups.FindAsync(id)).Name;
        }


        public Thread GetLastActiveThreadByGroupIdAsync(int groupId)
        {
            return Context.Groups.Include(g => g.Threads.Select(t => t.User)).SingleOrDefault(g => g.Id == groupId).Threads.OrderBy(t => t.Messages.Select(m => m.TimeStamp)).LastOrDefault();
        }


        /// <summary>
        /// Gets all the categories
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetGroupsCategoriesAsync()
        {
            ICollection<string> categories = new List<string>();

            // Goes true each group and adds the category to the list of its a new category
            await Context.Groups.ForEachAsync(g => categories.AddIfNew(g.Category));

            // Returns all the categories
            return categories.ToArray();
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

