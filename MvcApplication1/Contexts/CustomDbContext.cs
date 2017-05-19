using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication1.Contexts
{
    public class CustomDbContext:DbContext
    {
        public CustomDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ProfileModel> ProfileModel { get; set; }
    }
}