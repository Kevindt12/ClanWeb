using ClanWeb.Data.Entities.Forum;
using ClanWeb.Data.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ClanWeb.Core.Forums
{
    public  class PrivateMessageManager : IDisposable
    {


        public PrivateMessageManager()
        {
                
        }


        /// <summary>
        /// Finds a thread by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PrivateMessage> FindByIdAsync(int id)
        {
            return await new DatabaseContext().PrivateMessages.FindAsync(id);
        }



        /// <summary>
        /// Creates a new Thread
        /// </summary>
        /// <param name="privateMessage"></param>
        /// <returns></returns>
        public async Task CreatePrivateMessage(PrivateMessage privateMessage)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.PrivateMessages.Add(privateMessage);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates a exsisting thread
        /// </summary>
        /// <param name="privateMessage"></param>
        /// <returns></returns>
        public async Task UpdatePrivateMessage(PrivateMessage privateMessage)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.PrivateMessages.Add(privateMessage);
                context.Entry(privateMessage).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a thread
        /// </summary>
        /// <param name="privateMessage"></param>
        /// <returns></returns>
        public async Task DeleteThread(PrivateMessage privateMessage)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.PrivateMessages.Remove(privateMessage);
                await context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
