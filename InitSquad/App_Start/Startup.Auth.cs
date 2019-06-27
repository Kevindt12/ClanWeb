using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using ClanWeb.Core.Identity;
using ClanWeb.Core.Infomation;
using ClanWeb.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ClanWeb.Core.Forums;
using ClanWeb.Core.Extentions;


namespace ClanWeb.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<UserManager>(UserManager.Create);
            app.CreatePerOwinContext(() => new RoleManager());
            app.CreatePerOwinContext<SignInManager>(SignInManager.Create);
            app.CreatePerOwinContext(() => new NewsManager());
            app.CreatePerOwinContext(() => new ClanEventManager());
            app.CreatePerOwinContext(() => new ForumService());
            app.CreatePerOwinContext(() => new ReputationSystem());
            app.CreatePerOwinContext(() => new ForumUserManager());
            app.CreatePerOwinContext(() => new GroupManager());
            app.CreatePerOwinContext(() => new MessageManager());
            app.CreatePerOwinContext(() => new PrivateMessagingSystem());
            app.CreatePerOwinContext(() => new ThreadManager());


            
               new RoleManager().DefaultSeed();
            


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));


            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        }



        private void Seed()
        {
            UserManager userManager = new UserManager();

            User user = new User { UserName = "Kevindt12", Email = "Foo@Foo.com" };


            IdentityResult result = userManager.Create(user, "Kevindt12");

            if (result.Succeeded)
            {
                // Create a forum account for the user
                new ForumUserManager().Create(user);

            }

        }
    }







    // This class holds all the roles in the program
    public static class RolesConvention
    {
        // TODO: Use this and expand this and comment it is for saving the roles in a whay that its hard to mess it up


        /// <summary>
        /// All the base roles in the application
        /// </summary>
        public static class BaseRoles
        {
            public const string Administrator = "Administrator";
            public const string Moderator = "Moderator";
            public const string Member = "Member";

            public static List<string> GetRoles()
            {
                List<string> result = new List<string>();

                // Getting all the varibles names and saving them in a list
                List<object> varible = MethodBase.GetCurrentMethod()
                    .DeclaringType
                    .GetFields()
                    .Select(field => field.GetValue(MethodBase.GetCurrentMethod()))
                    .ToList();

                // Converting the object list to a string list
                foreach (object prop in varible)
                {
                    result.Add((string)prop);
                }

                return result;
            }

        }

        /// <summary>
        /// All the sub roles in the application
        /// </summary>
        public static class AdvancedRoles
        {
            // Admin
            public const string GenerateCode = "GenerateCode";
            public const string EditUser = "EditUser";
            public const string EditUserRoles = "EditUserRoles";
            public const string DeleteUser = "DeleteUser";
            public const string CreateNewsArticle = "CreateNewsArticle";
            public const string EditNewsArticle = "EditNewsArticle";
            public const string DeleteNewsArticle = "DeleteNewsArticle";
            public const string CreateEvent = "CreateEvent";
            public const string EditEvent = "EditEvent";
            public const string DeleteEvent = "DeleteEvent";
            public const string DeleteGroup = "DeleteGroup";
            public const string DeleteThread = "DeleteThread";
            public const string EditGroup = "EditGroup";
            public const string AddGroup = "AddGroup";


            public static List<string> GetRoles()
            {
                List<string> result = new List<string>();

                // Getting all the varibles names and saving them in a list
                List<object> varible = MethodBase.GetCurrentMethod()
                    .DeclaringType
                    .GetFields()
                    .Select(field => field.GetValue(MethodBase.GetCurrentMethod()))
                    .ToList();

                // Converting the object list to a string list
                foreach (object prop in varible)
                {
                    result.Add((string)prop);
                }

                return result;
            }
        }

        /// <summary>
        /// Getting the list of ALL roles
        /// </summary>
        /// <returns>List of strings with all the roles</returns>
        public static List<string> GetAllRoles()
        {
            List<string> result = new List<string>();

            result.AddRange(BaseRoles.GetRoles());
            result.AddRange(AdvancedRoles.GetRoles());

            return result;
        }


        public static string AdminAnd(string advrole)
        {
            return BaseRoles.Administrator + ", " + advrole;

        }

        public static string AdminAndModAnd(string advrole)
        {
            return BaseRoles.Administrator + ", " + BaseRoles.Moderator + ", " + advrole;

        }


    }





}