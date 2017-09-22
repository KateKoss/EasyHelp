using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication1.Contexts
{
    //public class MyInitializer : DropCreateDatabaseAlways<CustomDbContext>
    //{
    //    protected override void Seed(MagnateContext context)
    //    {
    //        // seed database here
    //    }
    //}
    public class CustomDbContext:DbContext
    {
        public CustomDbContext()
            : base("DefaultConnection")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<CustomDbContext>());
            Database.SetInitializer<CustomDbContext>(null);
        }
        
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ProfileModel> ProfileModel { get; set; }
        public DbSet<ArticleModel> ArticleModel { get; set; }        
        public DbSet<RequestModel> RequestsModel { get; set; }
        public DbSet<FindAssosiatesModel> FindAssosiatesModel { get; set; }
    }
}