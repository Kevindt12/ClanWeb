using System;
using System.Drawing;
using System.Configuration;
using System.IO;

namespace ClanWeb.Core.Identity
{
    public class ProfilePictureSystem : IProfilePictureSystem
    {

        private string _profilePictureLocation;
        private string _extention;

        /// <summary>
        /// The location of where the profile pictures are stored
        /// </summary>
        public string ProfilePictureLocations
        {
            get
            {
                return _profilePictureLocation ?? ConfigurationManager.AppSettings["ProfilePicturesLocation"];
            }
            private set
            {
                _profilePictureLocation = value;
            }
        }

        /// <summary>
        /// Gets the extention of the of the type of image
        /// </summary>
        public string Extention
        {
            get
            {
                return _extention ?? ".jpg";
            }

            private set
            {
                _extention = value;
            }
        }


        /// <summary>
        /// This is the system for managing all the profile pictures
        /// </summary>
        public ProfilePictureSystem()
        {

        }


        /// <summary>
        /// Assembles a profile picture file location
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string AssemleFileLocation(string userId)
        {
            return this.ProfilePictureLocations + "\\" + userId + this.Extention;
        }


        /// <summary>
        /// Saves a bitmap to the file location if there is already a file it will just override
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bitmap"></param>
        public void CreateOrUpdateProfilePicture(string userId, Bitmap bitmap)
        {
            string fileLocation = AssemleFileLocation(userId);
            bitmap.Save(fileLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /// <summary>
        /// Gets a profile picture file location (This is from the root of the application, not the server)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetProfilePicture(string userId)
        {
            // Make sure that the profile pricure exists
            if (File.Exists(AssemleFileLocation(userId)))
            {
                // Return the profile picture
                return AssemleFileLocation(userId);
            }
            else
            {
                // Return the default profile picture
                return "~/Content/Images/profile-default.png";
            }


        }

        /// <summary>
        /// Deletes a profile picture
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveProfilePicture(string userId)
        {
            try
            {
                File.Delete(AssemleFileLocation(userId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
