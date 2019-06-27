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
    public class ReputationSystem : IDisposable
    {
        public string CurrentUserId
        {
            get
            {
                return HttpContext.Current.User.Identity.GetUserId() ?? null;
            }
        }

        public DatabaseContext Context { get; set; }

        public ReputationSystem()
        {
            Context = new DatabaseContext();
        }

        /// <summary>
        /// Gets the general repucation from a thread
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public async Task<int> GetThreadReputationAsync(Thread thread)
        {
            int threadReputation = 0;
            await Task.Run(() => thread.Messages.ToList().ForEach(m => threadReputation += m.Reputation));

            return threadReputation;
        }

        /// <summary>
        /// Gets the current reputation of the message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<int> GetMessageReputationAsync(int messageId)
        {

            return (await Context.Messages.FindAsync(messageId)).Reputation;


        }


        /// <summary>
        /// Gets the current reputation of the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<int> GetMessageReputationAsync(Message message)
        {
            return await GetMessageReputationAsync(message.Id);
        }



        /// <summary>
        /// Checks to see if the current user can vote
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public  bool CanUserVote(Message message)
        {
            return CanUserVote(message.Id);
        }

        /// <summary>
        /// Checks if the user can vote on a message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public  bool CanUserVote(int messageId)
        {
            return !Context.ForumUsers.Any(u => u.VotedMessages.Any(m => m.Id == messageId));
        }

        /// <summary>
        /// Upvotes a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<int> UpVoteOnMessageAsync(Message message)
        {
            return await UpVoteOnMessageAsync(message.Id);

        }

        /// <summary>
        /// Upvotes a message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<int> UpVoteOnMessageAsync(int messageId)
        {
            // Getting the user
            ForumUser user = await Context.ForumUsers.FindAsync(CurrentUserId);

            // Getting the message and we will change it in scope
            Message message = await Context.Messages.FindAsync(messageId);

            // Adding the message reputation
            message.Reputation++;
            message.Thread.Reputation++;
            user.VotedMessages.Add(message);
            await Context.SaveChangesAsync();

            return message.Reputation;

        }

        /// <summary>
        /// Down votes a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<int> DownVoteOnMessageAsync(Message message)
        {
            return await DownVoteOnMessageAsync(message.Id);
        }

        /// <summary>
        /// Down votes a message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<int> DownVoteOnMessageAsync(int messageId)
        {

            // Getting the user
            ForumUser user = await Context.ForumUsers.FindAsync(CurrentUserId);

            // Getting the message and we will change it in scope
            Message message = await Context.Messages.FindAsync(messageId);

            // Adding the message reputation
            message.Reputation--;
            message.Thread.Reputation--;
            user.VotedMessages.Add(message);
            await Context.SaveChangesAsync();

            return message.Reputation;

        }






        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }




        }
    }
}
