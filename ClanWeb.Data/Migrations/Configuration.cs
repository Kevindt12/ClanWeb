namespace ClanWeb.Data.Migrations
{
    using ClanWeb.Data.Entities;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ClanWeb.Data.Repository.EntityFramework.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ClanWeb.Data.Repository.EntityFramework.DatabaseContext";
        }

        protected override void Seed(ClanWeb.Data.Repository.EntityFramework.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.


            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
