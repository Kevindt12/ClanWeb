using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

using InitSquad.Controllers;
using InitSquad.Models;
using InitSquad.Models.ViewModels;
using InitSquad.ApplicationCode.UI.Web;
using InitSquad.ApplicationCode.Managers;
using InitSquad.AppCode.Results;
using System.Web.Mvc;
using System.Security.Principal;
using System.IO;
using System.Drawing;

namespace InitSquad.AppCode.Managers
{
    public class AppEventsManager : Manager
    {
 
        // Constructor
        public AppEventsManager(Controller controller) 
            : base(controller)
        { 

        }


        /// <summary>
        /// Finds a single event and returns it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventBlock GetEventById(int id)
        {
            return Db.Events.Find(id);
        }


        /// <summary>
        /// Retuns all the events in the database
        /// </summary>
        /// <returns>List of events</returns>
        public List<EventBlock> GetAllEvents()
        {
            // Gets all the events from the database
            List<EventBlock> events = Db.Events.ToList();

            return events;
        }



        /// <summary>
        /// Retuns a Event View model to be used directly
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event View Model</returns>
        public EventListingViewModel GetSingleEventListingViewModel(int id)
        {
            // Searches for the event
            EventBlock singleEvent = Db.Events.Find(id);

            // Converts the event to a View Model
            EventListingViewModel model = new EventListingViewModel
            {
                EventName = singleEvent.EventName,
                EventDate = singleEvent.EventDateAndTime
            };

            return model;
        }



        /// <summary>
        /// Finds who made the event
        /// </summary>
        /// <param name="Eventid"></param>
        /// <returns>A Application user</returns>
        public ApplicationUser GetUserWhoCreatedTheEvent(int Eventid)
        {
            return Db.Events.Find(Eventid).User;
        }


        /// <summary>
        /// Adds a event to the database
        /// </summary>
        /// <param name="model">The model data</param>
        public void AddEvent(EventBlock model)
        {
            EventBlock singleEvent = new EventBlock
            {
                EventName = model.EventName,
                EventDateAndTime = model.EventDateAndTime,
                Game = model.Game,
                Description = model.Description,
                ImageLocation = model.ImageLocation,
                Location = model.Location,
                User = GetCurrenctUser()
            };

            // Add the entry to the database
            Db.Events.Add(singleEvent);
            Db.SaveChanges();
        }

        /// <summary>
        /// Edits a event in the database
        /// </summary>
        /// <param name="model">A event block</param>
        public void UpdateEvent(EventBlock model)
        {
            // Setting the varibles
            Db.Events.Find(model.Id).EventName = model.EventName;
            Db.Events.Find(model.Id).EventDateAndTime = model.EventDateAndTime;

            // Also saves the name of the last person who edited the event
            Db.Events.Find(model.Id).LastPersonToEdit = GetCurrenctUser();
            Db.Events.Find(model.Id).ImageLocation = model.ImageLocation;

            // We dont care about this now
            //Db.Events.Find(model.Id).Location = model.Location;
            //Db.Events.Find(model.Id).Game = model.Game;
            //Db.Events.Find(model.Id).User = model.User;
            //Db.Events.Find(model.Id).LastPersonToEdit = model.LastPersonToEdit;

            // Saving this to the database
            Db.SaveChanges();
        }


        /// <summary>
        /// Deletes a event from the database
        /// </summary>
        /// <param name="singleEvent">The event that need to be deleted As EventBlock</param>
        public void DeleteEvent(EventBlock singleEvent)
        {
            // Deleting the event
            Db.Events.Remove(singleEvent);
            Db.SaveChanges();

        }

        /// <summary>
        /// Deletes a event from the database
        /// </summary>
        /// <param name="id">The Id of the event</param>
        public void DeleteEvent(int id)
        {
            // Getting the item
            EventBlock singleEvent = Db.Events.Find(id);

            // Deleting the item
            Db.Events.Remove(singleEvent);
            Db.SaveChanges();
        }


        /// <summary>
        /// Gets a new location with string where a new image can be saved
        /// </summary>
        /// <returns>String URL Location</returns>
        public string SaveEventImage(Bitmap bitmap)
        {
            // Getting and setting the varibles
            string folderUrl = "\\Content\\Images\\EventImages\\";
            string fileName = new AppGenerateCodeManager(base.Controller).GenerateCode(45) + ".jpg";
            string fullSaveLocation = Controller.Server.MapPath(folderUrl) + fileName;

            // Creating the folder or checking if it is there
            CreateFolderIfNotExists(folderUrl);

            // saving the image
            bitmap.Save(fullSaveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Only return the path from root
            return folderUrl + fileName;



        }

        






    }

}