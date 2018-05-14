using System.Data.Entity;
using DAL.Entities;
using DAL.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.EF
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        static DataContext()
        {
            Database.SetInitializer((new DbInitializaer()));
        }

        public DataContext(string connectionString) : base(connectionString) {}
    }

    public class DbInitializaer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            string[] commands =
            {
                @"INSERT INTO [dbo].[AspNetRoles] ([Id] ,[Name] ,[Discriminator]) VALUES ('9c39f82e-c87b-4bfb-b786-eec87ac45744','admin','ApplicationRole')",
                @"INSERT INTO [dbo].[AspNetRoles] ([Id] ,[Name] ,[Discriminator]) VALUES ('3b25e968-41c8-48cd-bd65-7f235b5c209e','user','ApplicationRole')",
                @"INSERT INTO [dbo].[AspNetRoles] ([Id] ,[Name] ,[Discriminator]) VALUES ('cb25e968-4dc8-48cd-ac65-7f230a5c209e','moderator','ApplicationRole')",
                @"INSERT INTO [dbo].[AspNetUsers] ([Id] ,[Email] ,[EmailConfirmed] ,[PasswordHash] ,[SecurityStamp] ,[PhoneNumber] ,[PhoneNumberConfirmed] ,[TwoFactorEnabled],[LockoutEndDateUtc] ,[LockoutEnabled] ,[AccessFailedCount] ,[UserName]) 
                    VALUES ('f0e55c71-2e6c-41df-bea1-1ff5dc102f59' ,'admin@gmail.com' ,0 ,'AGWw56+JXBJWPXED3BLTjDmPM5/166S/TuwnjGG842sBb1a967GidG+05UWJRukNXw==' ,'1c52b5dd-622b-47a6-b426-46e4751c548b',NULL ,0 ,0 ,NULL ,0 ,0 ,'admin@gmail.com')", //password = "adminpassword"
                @"INSERT INTO [dbo].[Users] ([Id], [Name]) VALUES ('f0e55c71-2e6c-41df-bea1-1ff5dc102f59', 'Admin')",
                @"INSERT INTO [dbo].[AspNetUserRoles] ([UserId] ,[RoleId]) VALUES ('f0e55c71-2e6c-41df-bea1-1ff5dc102f59', '9c39f82e-c87b-4bfb-b786-eec87ac45744')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Auto parts, Tuning, GPS')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Audio, Radio')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Business, Industry, Services')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Grocery')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Appliances')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('For children')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('For repair, Tools')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Cottage, Garden, Backyard')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('House, Interior')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Plants')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Antiques')",
                @"INSERT INTO [dbo].[Categories] ([Name]) VALUES ('Other')"
            };

            foreach (var el in commands)
                context.Database.ExecuteSqlCommand(el);

            context.SaveChanges();
        }
    }
}
