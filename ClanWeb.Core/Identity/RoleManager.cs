using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using ClanWeb.Data.Repository.EntityFramework;
using ClanWeb.Data.Entities;

namespace ClanWeb.Core.Identity
{
    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager() : base(new RoleStore<IdentityRole>())
        {

        }


        /// <summary>
        /// Gets all primary roles
        /// </summary>
        /// <returns>A list of primary roles</returns>
        public async Task<IEnumerable<string>> GetPrimaryRoles()
        {
            ICollection<string> roles = new List<string>();

            // Checks and adds all the primary roles into a list
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Users.ToList().ForEach((u) => roles.AddIfNew(u.PrimaryRole));
            }

            return roles;
        }


        /// <summary>
        /// Seeds the database with the default roles it sould have
        /// </summary>
        public async void DefaultSeed()
        {

            // Base Roles
            // Members Role
            await AddRoleIfNotExists("Members");

            // Moderators Role
            await AddRoleIfNotExists("Moderators");

            // Administrators Role
            await AddRoleIfNotExists("Administrators");


            // Section Roles

            // Administrators Role
            await AddRoleIfNotExists("SeeUsers");
            // Administrators Role
            await AddRoleIfNotExists("ApproveUsers");
            // Administrators Role
            await AddRoleIfNotExists("EditUserRole");
            // Administrators Role
            await AddRoleIfNotExists("EditUsers");
            // Administrators Role
            await AddRoleIfNotExists("DeleteUsers");
            // Administrators Role
            await AddRoleIfNotExists("BanUsers");

            // Administrators Role
            await AddRoleIfNotExists("SeeNewsArticles");
            // Administrators Role
            await AddRoleIfNotExists("CreateNewsArticles");
            // Administrators Role
            await AddRoleIfNotExists("EditNewsArticles");
            // Administrators Role
            await AddRoleIfNotExists("DeleteNewsArticles");

            // Administrators Role
            await AddRoleIfNotExists("SeeEvents");
            // Administrators Role
            await AddRoleIfNotExists("CreateEvents");
            // Administrators Role
            await AddRoleIfNotExists("EditEvents");
            // Administrators Role
            await AddRoleIfNotExists("DeleteEvents");

            // Administrators Role
            await AddRoleIfNotExists("ManageGroups");
            // Administrators Role
            await AddRoleIfNotExists("ThreadModerator");


        }

        private async Task AddRoleIfNotExists(string RoleName)
        {
            if (!(await RoleExistsAsync(RoleName)))
            {
                await CreateAsync(new IdentityRole { Name = RoleName });
            }
        }


       



    }
}
