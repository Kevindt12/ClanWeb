using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Configuration;

using ClanWeb.Data.Entities;
using ClanWeb.Data.Repository.EntityFramework;
using System.Drawing.Imaging;
using ClanWeb.Core.Identity;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ClanWeb.Core.Infomation
{
    public class ClanEventManager : IDisposable
    {

        private string _eventImagesLocation;
        private string _imageExtention;
        private Size? _maxImageSize;

        /// <summary>
        /// The location where all the images of the events are saved
        /// </summary>
        public string EventImagesLocation
        {
            get
            {
                return _eventImagesLocation ?? ConfigurationManager.AppSettings["EventsImagesLocation"];
            }
            set
            {
                _eventImagesLocation = value;
            }
        }


        /// <summary>
        /// Gets the extention used for the images
        /// </summary>
        public string ImageExtention
        {
            get { return _imageExtention ?? ".jpg"; }
            set { _imageExtention = value; }
        }

        public DatabaseContext Context { get; set; }


        /// <summary>
        /// Gets or sets the max size of a image
        /// </summary>
        public Size MaxImageSize
        {
            get
            {
                return _maxImageSize ?? new Size(360, 360);
            }
            set
            {
                _maxImageSize = value;
            }
        }



        /// <summary>
        /// Creates a new manager for the events of the clan
        /// </summary>
        public ClanEventManager()
        {
            Context = new DatabaseContext();
        }


        /// <summary>
        /// Creates a new manager for the events of the clan
        /// </summary>
        /// <param name="eventImagesLocation">The location of where the event images should be stored</param>
        public ClanEventManager(string eventImagesLocation)
        {
            EventImagesLocation = eventImagesLocation;
        }


        /// <summary>
        /// Saves a image for the events
        /// </summary>
        /// <returns></returns>
        private string SaveEventImage(Image image)
        {
            // Creating the image location
            string fileLocation = EventImagesLocation + "\\" + Guid.NewGuid().ToString() + ImageExtention;

            // Resizing the image
            ResizeImage(image, MaxImageSize);

            // Trying to save the image
            try
            {
                image.Save(fileLocation, ImageFormat.Jpeg);
                return fileLocation;
            }
            catch (ExternalException eex)
            {
                // TODO: Create logging for the application
                throw eex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Make sure we dispose of the image on the end
                image.Dispose();
                image = null;
            }
        }

        /// <summary>
        /// Saves a image for the events
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string SaveEventImage(Image image, string fileName)
        {
            // Creating the image location
            string fileLocation = EventImagesLocation + "\\" + fileName + ImageExtention;

            // Resizing the image
            ResizeImage(image, MaxImageSize);

            // Trying to save the image
            try
            {
                image.Save(fileLocation, ImageFormat.Jpeg);
                return fileLocation;
            }
            catch (ExternalException eex)
            {
                // TODO: Create logging for the application
                throw eex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Make sure we dispose of the image on the end
                image.Dispose();
                image = null;
            }
        }


        /// <summary>
        /// Resizes a image to a new size
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        private Image ResizeImage(Image image, Size maxSize)
        {
            // Checks if the size has surpassed the max size
            if (image.Height > maxSize.Height || image.Width > maxSize.Width)
            {

                int totalProcentageOfDecrease = 0;

                // Calculate the procentage of decrease
                int heightProcentage = (image.Height - maxSize.Height) / image.Height;
                int witdhProcentage = (image.Width - maxSize.Width) / image.Width;

                // Pick the highes value of decrease
                totalProcentageOfDecrease = Math.Min(heightProcentage, witdhProcentage);

                // Create a new size
                Size newSize = new Size((totalProcentageOfDecrease * image.Width), (totalProcentageOfDecrease * image.Height));

                // Return the image with the new size
                return (Image)(new Bitmap(image, newSize));
            }
            else
            {
                return image;
            }
        }


        /// <summary>
        /// Gets a event by a given id
        /// </summary>
        /// <param name="id">The if of the event</param>
        /// <returns>A Event</returns>
        public async Task<ClanEvent> FindByIdAsync(int id)
        {
            return await Context.ClanEvents.AsNoTracking().SingleOrDefaultAsync(ce => ce.Id == id);
        }


        /// <summary>
        /// Returns all the clan events
        /// </summary>
        /// <returns>all the events in a list</returns>
        public IEnumerable<ClanEvent> GetClanEvents()
        {

            return Context.ClanEvents.AsNoTracking().ToList();
        }


        /// <summary>
        /// Returns all the clan events
        /// </summary>
        /// <returns>all the events in a list</returns>
        public async Task<IEnumerable<ClanEvent>> GetClanEventsAsync()
        {
            return await Context.ClanEvents.AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="entity">The data that needs to passed true</param>
        public async Task CreateClanEventAsync(ClanEvent entity)
        {
            entity.CreatedBy = Context.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            Context.ClanEvents.Add(entity);
            await Context.SaveChangesAsync();

        }

        /// <summary>
        /// Creates a new event and saves the image for that event aslo
        /// </summary>
        /// <param name="entity">Tyhe event that needs to be saved</param>
        /// <param name="image">The image that goes with that event</param>
        public async Task CreateClanEventAsync(ClanEvent entity, Image image)
        {
            entity.CreatedBy = Context.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            entity.ImageLocation = SaveEventImage(image);
            Context.ClanEvents.Add(entity);
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Updating the event
        /// </summary>
        /// <param name="entity">the entity that has to be changed</param>
        public async Task UpdateClanEventAsync(ClanEvent entity)
        {
            Context.ClanEvents.Attach(entity);
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the event and its image with the event
        /// </summary>
        /// <param name="entity">Tyhe event that needs to be saved</param>
        /// <param name="image">The image that goes with that event</param>
        public async Task UpdateClanEventAsync(ClanEvent entity, Image image)
        {
            entity.ImageLocation = SaveEventImage(image);
            Context.ClanEvents.Attach(entity);
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            await Context.SaveChangesAsync();
        }


        /// <summary>
        /// Removes a entity from the applciation
        /// </summary>
        /// <param name="entity">the event that needs to be removed</param>
        public async Task DeleteClanEventAsync(ClanEvent entity)
        {
            Context.ClanEvents.Remove(entity);
            await Context.SaveChangesAsync();
        }



        public virtual void Dispose()
        {

        }
    }
}
