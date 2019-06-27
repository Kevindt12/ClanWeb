using ClanWeb.Data.Entities;
using ClanWeb.Data.Entities.Forum;
using ClanWeb.Data.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ClanWeb.Core.Forums
{
    public class MessageManager : IDisposable
    {

        public MessageManager()
        {

        }


        /// <summary>
        /// Creates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Message> CreateAsync(Message message, Thread parrent)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                message.TimeStamp = DateTime.Now;

                context.Messages.Add(message);
                await context.SaveChangesAsync();
                return message;
            }
        }

        /// <summary>
        /// Finds a message by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Message> FindByIdAsync(int id)
        {
            return await new DatabaseContext().Messages.FindAsync(id);

        }

        /// <summary>
        /// Gets all the message in the application
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await new DatabaseContext().Messages.ToListAsync();
        }


        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Message message)
        {
            using (DatabaseContext context = new DatabaseContext())
            {


                // Saving the message and uploading everything to the database
                context.Messages.Add(message);
                context.Entry(message).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a message from the application
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Message message)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Messages.Remove(message);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a message from the application
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int messageId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Message message = await context.Messages.FindAsync(messageId);
                context.Messages.Remove(message);
                await context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
