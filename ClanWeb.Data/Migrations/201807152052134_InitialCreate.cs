namespace ClanWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClanEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        EventDateAndTime = c.DateTime(nullable: false),
                        Game = c.String(),
                        Location = c.String(),
                        ImageLocation = c.String(),
                        Description = c.String(),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id, cascadeDelete: true)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RankLevel = c.String(),
                        Country = c.String(maxLength: 40),
                        Seniority = c.String(),
                        PrimaryRole = c.String(),
                        IsInReview = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ForumUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Reputation = c.Int(nullable: false),
                        PostsCount = c.Int(nullable: false),
                        Signature = c.String(),
                        LastActivity = c.DateTime(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Reputation = c.Int(nullable: false),
                        MessagePlacementInThread = c.Int(nullable: false),
                        Thread_Id = c.Int(nullable: false),
                        User_UserId = c.String(maxLength: 128),
                        ForumUser_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.Thread_Id, cascadeDelete: true)
                .ForeignKey("dbo.ForumUsers", t => t.User_UserId)
                .ForeignKey("dbo.ForumUsers", t => t.ForumUser_UserId)
                .Index(t => t.Thread_Id)
                .Index(t => t.User_UserId)
                .Index(t => t.ForumUser_UserId);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ViewCount = c.Int(nullable: false),
                        LastEdited = c.DateTime(nullable: false),
                        Pinned = c.Boolean(nullable: false),
                        Reputation = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                        User_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.ForumUsers", t => t.User_UserId)
                .Index(t => t.Group_Id)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ShortText = c.String(),
                        LongText = c.String(),
                        Date = c.DateTime(nullable: false),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id, cascadeDelete: true)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.PrivateMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(),
                        Message = c.String(),
                        SendTime = c.DateTime(nullable: false),
                        From_UserId = c.String(maxLength: 128),
                        To_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumUsers", t => t.From_UserId)
                .ForeignKey("dbo.ForumUsers", t => t.To_UserId)
                .Index(t => t.From_UserId)
                .Index(t => t.To_UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SubscriptionsSubscribers",
                c => new
                    {
                        Subscribers = c.String(nullable: false, maxLength: 128),
                        Subscriptions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subscribers, t.Subscriptions })
                .ForeignKey("dbo.ForumUsers", t => t.Subscribers, cascadeDelete: true)
                .ForeignKey("dbo.Threads", t => t.Subscriptions, cascadeDelete: true)
                .Index(t => t.Subscribers)
                .Index(t => t.Subscriptions);
            
            CreateTable(
                "dbo.VotedOnMessages",
                c => new
                    {
                        Messages_User = c.String(nullable: false, maxLength: 128),
                        User_Message = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Messages_User, t.User_Message })
                .ForeignKey("dbo.ForumUsers", t => t.Messages_User, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.User_Message, cascadeDelete: true)
                .Index(t => t.Messages_User)
                .Index(t => t.User_Message);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PrivateMessages", "To_UserId", "dbo.ForumUsers");
            DropForeignKey("dbo.PrivateMessages", "From_UserId", "dbo.ForumUsers");
            DropForeignKey("dbo.NewsArticles", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VotedOnMessages", "User_Message", "dbo.Messages");
            DropForeignKey("dbo.VotedOnMessages", "Messages_User", "dbo.ForumUsers");
            DropForeignKey("dbo.ForumUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionsSubscribers", "Subscriptions", "dbo.Threads");
            DropForeignKey("dbo.SubscriptionsSubscribers", "Subscribers", "dbo.ForumUsers");
            DropForeignKey("dbo.Messages", "ForumUser_UserId", "dbo.ForumUsers");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.ForumUsers");
            DropForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Threads", "User_UserId", "dbo.ForumUsers");
            DropForeignKey("dbo.Threads", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.ClanEvents", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.VotedOnMessages", new[] { "User_Message" });
            DropIndex("dbo.VotedOnMessages", new[] { "Messages_User" });
            DropIndex("dbo.SubscriptionsSubscribers", new[] { "Subscriptions" });
            DropIndex("dbo.SubscriptionsSubscribers", new[] { "Subscribers" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PrivateMessages", new[] { "To_UserId" });
            DropIndex("dbo.PrivateMessages", new[] { "From_UserId" });
            DropIndex("dbo.NewsArticles", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Threads", new[] { "User_UserId" });
            DropIndex("dbo.Threads", new[] { "Group_Id" });
            DropIndex("dbo.Messages", new[] { "ForumUser_UserId" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "Thread_Id" });
            DropIndex("dbo.ForumUsers", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ClanEvents", new[] { "CreatedBy_Id" });
            DropTable("dbo.VotedOnMessages");
            DropTable("dbo.SubscriptionsSubscribers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PrivateMessages");
            DropTable("dbo.NewsArticles");
            DropTable("dbo.Groups");
            DropTable("dbo.Threads");
            DropTable("dbo.Messages");
            DropTable("dbo.ForumUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ClanEvents");
        }
    }
}
