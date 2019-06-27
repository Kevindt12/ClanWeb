using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using System.Data;

using ClanWeb.Data.Entities;
using ClanWeb.Data.Repository.EntityFramework;
using ClanWeb.Core.Identity;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ClanWeb.Core.Infomation
{
    public class NewsManager : IDisposable
    {

        public DatabaseContext Context { get; set; }

        /// <summary>
        /// This class handels everything to do with news
        /// </summary>
        public NewsManager()
        {
            Context = new DatabaseContext();
        }



        /// <summary>
        /// Returns a news article
        /// </summary>
        /// <param name="id">The id of the article</param>
        public async Task<NewsArticle> FindByIdAsync(int id)
        {
            return await Context.News.AsNoTracking().SingleOrDefaultAsync(n => n.Id == id);
        }


        /// <summary>
        /// Gets all the news articles
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync()
        {
            return await Context.News.AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Gets all the news articles
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewsArticle> GetNewsArticles()
        {
            return Context.News.AsNoTracking().ToList();
        }


        /// <summary>
        /// Creates a new news article
        /// </summary>
        /// <param name="newsArticle"></param>
        public async Task CreateNewsArticleAsync(NewsArticle newsArticle)
        {                 
            newsArticle.ShortText = CreateShortText(newsArticle.LongText);
            newsArticle.CreatedBy = Context.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            newsArticle.Date = DateTime.Now;
            Context.News.Add(newsArticle);
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Updates a news article
        /// </summary>
        /// <param name="newsArticle"></param>
        public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
        {
            // Creating a new short text for the article
            newsArticle.ShortText = await Task.Run<string>(() => CreateShortText(newsArticle.LongText));

            // Saving everything to the database
            Context.News.Attach(newsArticle);
            Context.Entry(newsArticle).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a news article from the applciation
        /// </summary>
        /// <param name="newsArticle"></param>
        public async Task DeleteNewsArticleAsync(NewsArticle newsArticle)
        {
                Context.News.Remove(newsArticle);
                await Context.SaveChangesAsync();
        }



        /// <summary>
        /// Creates the short text for the news articles
        /// </summary>
        /// <param name="longText">The long text or the whole news article</param>
        /// <returns>a +- 300 char version of it</returns>
        private string CreateShortText(string longText)
        {
            const int charsInShortText = 300;
            const string endChar = "... </p>";

            // Creates the basis of the string with only 300 char
            string shortText = longText.Substring(0, charsInShortText);

            // Creates a list of all the words
            List<string> shortTextPeces = new List<string>(shortText.Split(' '));

            // Removes the last work because its most proboly cut off
            shortTextPeces.RemoveAt(shortText.Split(' ').Length - 1);

            // Joins all the words back togather
            shortText = String.Join<string>(" ", shortTextPeces);

            // Adds the end paragraph text 
            shortText += endChar;

            return shortText;
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
