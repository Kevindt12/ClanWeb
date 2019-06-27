using System.Drawing;

namespace ClanWeb.Core.Identity
{
    public interface IProfilePictureSystem
    {

        /// <summary>
        /// The location of all the profile pictures in the application
        /// </summary>
        string ProfilePictureLocations { get; }

        /// <summary>
        /// Gets a single profile picture based on the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetProfilePicture(string userId);

        /// <summary>
        /// Removes a profile picture
        /// </summary>
        /// <param name="userId"></param>
        void RemoveProfilePicture(string userId);

        /// <summary>
        /// Creates a new profile picture
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bitmap"></param>
        void CreateOrUpdateProfilePicture(string userid, Bitmap bitmap);



    }
}
