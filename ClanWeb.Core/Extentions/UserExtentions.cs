using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using ClanWeb.Core.Identity;
using ClanWeb.Data.Entities;

namespace ClanWeb.Core.Extentions
{
    public static class UserExtentions
    {

        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this User user, UserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }


    }
}
