using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

using ClanWeb.Data.Entities;
using ClanWeb.Data.Repository.EntityFramework;
using System.Web;
using Microsoft.Owin;

namespace ClanWeb.Core.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class UserManager : UserManager<User>
    {

        /// <summary>
        /// This is the manager that handels all the users for the application
        /// </summary>
        public UserManager()
            : base(new UserStore<User>(new DatabaseContext()))
        {
            Construct();
        }

        /// <summary>
        /// This is the manager that handels all the users for the application
        /// </summary>
        public UserManager(IUserStore<User> store)
            : base(store)
        {

            Construct();
        }

    
        /// <summary>
        /// This will construct this class and set all its defualt values
        /// </summary>
        protected virtual void Construct()
        {
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            this.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            this.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            IDataProtectionProvider dataProtectionProvider = new IdentityFactoryOptions<UserManager>().DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            
        }



        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }


        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            // We are overriding this to make sure we allways work with all lower case
            user.Email.ToLower();

            // All users that get a account need to go true a approval process
            user.IsInReview = true;
            
            // Contenue with the base operation
            return await base.CreateAsync(user, password);
        }


        /// <summary>
        /// Finds a user by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override async Task<User> FindByEmailAsync(string email)
        {
            // We are overriding this to make sure we allways work with all lower case
            email.ToLower();

            return await base.FindByEmailAsync(email);
        }


        /// <summary>
        /// Sends a email when the person is regestring
        /// </summary>
        /// <param name="CallbackUrl"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task SendRegistrationEmailAsync(string CallbackUrl, string userId)
        {
            await this.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + CallbackUrl + "\">link</a>");
        }


        /// <summary>
        /// sends a email when the person nees to rest his password
        /// </summary>
        /// <param name="CallbackUrl"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task SendPasswordResetLinkAsync(string CallbackUrl, string userId)
        {
            await this.SendEmailAsync(userId, "Reset your password", "Please click the link to reset your password: <a href=\"" + CallbackUrl + "\">link</a>");
        }


        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetCurrentUserAsync()
        {
            using (UserManager userManager = new UserManager())
            {
                return await userManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            }
        }

        
        /// <summary>
        /// This will approve a user to access the platform and all of its features
        /// </summary>
        /// <param name="userId">The user that needs approving</param>
        public virtual async Task ApproveUserAsync(string userId)
        {
            User user = await Store.FindByIdAsync(userId);

            user.IsInReview = false;

            await Store.UpdateAsync(user);
        }


        /// <summary>
        /// This will denail the user to access the platform and delete there account
        /// </summary>
        /// <param name="userId"></param>
        public virtual async Task DisapproveUserAsync(string userId)
        {
            User user = await Store.FindByIdAsync(userId);

            user.IsInReview = true;

            await Store.DeleteAsync(user);
        }



    }
}
