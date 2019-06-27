using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

using ClanWeb.Data.Entities;
using ClanWeb.Data.Entities.Forum;
using System.Diagnostics;

namespace ClanWeb.Data.Repository.EntityFramework
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext()
            : base("DefaultConnection")
        {
            // Database Inilization
            //Database.SetInitializer<DatabaseContext>(new DropCreateDatabaseIfModelChanges<DatabaseContext>());

            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());

            Database.Log = message => Debug.WriteLine(message);
        }


        public DbSet<ClanEvent> ClanEvents { get; set; }
        public DbSet<NewsArticle> News { get; set; }
        public DbSet<ForumUser> ForumUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Group> Groups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Many to many for Subscriptions
            modelBuilder.Entity<ForumUser>()
                .HasMany<Thread>(s => s.Subscriptions)
                .WithMany(c => c.Subscribers)
                .Map(cs =>
                {
                    cs.MapLeftKey("Subscribers");
                    cs.MapRightKey("Subscriptions");
                    cs.ToTable("SubscriptionsSubscribers");
                });

            // Many to many for the voting system
            modelBuilder.Entity<ForumUser>()
                .HasMany<Message>(u => u.VotedMessages)
                .WithMany(m => m.UsersThatVoted)
                .Map(um =>
                {
                    um.MapLeftKey("Messages_User");
                    um.MapRightKey("User_Message");
                    um.ToTable("VotedOnMessages");
                });

            modelBuilder.Entity<Thread>()
                .HasOptional<ForumUser>(t => t.User)
                .WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasOptional<ForumUser>(m => m.User)
                .WithMany().WillCascadeOnDelete(false);


            modelBuilder.Entity<PrivateMessage>()
                .HasOptional<ForumUser>(pm => pm.To)
                .WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<PrivateMessage>()
                .HasOptional<ForumUser>(pm => pm.From)
                .WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

        }


    }
}
