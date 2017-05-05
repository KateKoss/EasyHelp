using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcApplication1.Filters;


namespace MvcApplication1.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index(ProfileModel model)
        {
            model.flag_about_me = !model.flag_about_me;
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Index(ProfileModel model, LoginModel lm )
        {
            
            //model.flag_about_me = !model.flag_about_me;
            //bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //if (ModelState.IsValid)
            {
                using (UsersContext db = new UsersContext())
                {
                    //
                    //db.SaveChanges();

                    //foreach (var blog in db.Blogs)
                    //{
                    //    Console.WriteLine(blog.Name);
                    //}                    
                    //var userDetails = db.UserProfiles.Where(x => x.UserName == lm.UserName && x.Password == model.Password).FirstOrDefault();
                    //if (lm.UserName != null)
                    //    model.UserID = WebSecurity.GetUserId(User.Identity.Name);
                    //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogContext, Configuration>());
                    //var currentPerson = db.UserProfiles.Where(p => p.UserID == Convert.ToInt32(Request.Cookies["Id"]));





                    var currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                    //var currentPerson = "fhdgsdfj";
                    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                    if (user != null)
                    {
                        db.UserProfiles.Add(new UserProfile { About_me = model.About_me });
                        //user.About_me = model.About_me;
                        db.SaveChanges();
                    }
                    else ModelState.AddModelError(currentPerson, currentPerson + "Error");
                }
                //TODO: SubscribeUser(model.Email);
            }

            return View("Index", model);
        }
    }
}
