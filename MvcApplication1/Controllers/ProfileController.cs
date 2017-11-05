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
using System.Text;
using System.IO;
using MvcApplication1.Contexts;
using System.Reflection;

namespace MvcApplication1.Controllers
{    
    public class ProfileController : Controller
    {
        Singleton s = Singleton.Instance;
        public ProfileController()
        {
            if (Request.Cookies["UserId"] != null)
                s.user = Convert.ToString(Request.Cookies["UserId"].Value);
        }
        
        [HttpGet]
        public ActionResult Index(ProfileModel model)
        { 
            if (s.user != null && s.user != "")
            {
                using (CustomDbContext db = new CustomDbContext())
                {

                    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == s.user);
                    if (user != null)
                    {
                        if (user.Role == "mentor")
                        {
                            return RedirectToAction("MentorProfile");
                        }
                        var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == s.user);
                        if (myModel.UserPhoto == null)
                        {

                        }
                        var list = new List<String>();
                        if (myModel.MyTegs != null)
                        {
                            var splitTegs = myModel.MyTegs.Split('|');

                            foreach (var el in splitTegs)
                            {
                                list.Add(el);

                            }
                        }
                        model = new ProfileModel()
                        {
                            Name = myModel.Name,
                            MyTegs = myModel.MyTegs,
                            About_me = myModel.About_me,
                            UserPhoto = myModel.UserPhoto,
                            searchMentor = model.searchMentor,
                            tegs = list
                        };
                    }
                    else model = new ProfileModel() { };
                }
            }
            return View("Index", model);
            
        }

        [HttpGet]
        public ActionResult GetMentorList()
        {
            ProfileModel model = new ProfileModel() { };

            List<ProfileModel> modelList = new List<ProfileModel>() { };
            if (s.user != null && s.user != "")
            {
                
                using (CustomDbContext db = new CustomDbContext())
                {
                    if (db.ProfileModel.SingleOrDefault(x => x.UserName == s.user).MyTegs != null)
                    {
                        string[] currentPersonTegs = db.ProfileModel.SingleOrDefault(x => x.UserName == s.user).MyTegs.Split('|');

                        List<ProfileModel> mentorListWithTegs = new List<ProfileModel>();
                        var mentorList = db.UserProfiles.Where(x => x.Role == "mentor");
                        if (mentorList != null)
                            foreach (var m in mentorList)
                                for (int i = 0; i < currentPersonTegs.Length; i++)
                                {
                                    string currentTeg = currentPersonTegs[i];
                                    if (currentTeg != "")
                                    {
                                        var currentMentor = m.UserName;
                                        using (CustomDbContext db2 = new CustomDbContext())
                                        {
                                            var ment = db2.ProfileModel.SingleOrDefault(x => x.UserName == currentMentor);
                                            if (ment != null)
                                                if (ment.UserName != s.user)
                                                    if (ment.MyTegs != null)
                                                        if (ment.MyTegs.Contains(currentTeg))
                                                            if (!mentorListWithTegs.Any(x => x.UserName == ment.UserName))
                                                                mentorListWithTegs.Add(ment);
                                        }
                                    }
                                }


                        foreach (var m in mentorListWithTegs)
                        {
                            model = new ProfileModel(); 
                            
                            model.Name = m.Name;
                            model.UserName = m.UserName;
                            model.UserPhoto = m.UserPhoto;
                            model.isMentor = true;

                            modelList.Add(model);
                        }
                    }
                }
            }
            return PartialView(modelList);
        }

        public ActionResult GetUserForChat()
        {
            List<ProfileModel> modelList = new List<ProfileModel>();
            if (s.user != null && s.user !="")
            {
                using (var db = new CustomDbContext())
                {
                    var list = db.ChatMessages.Where(x => x.FromUser == s.user).ToList();
                    foreach (var item in list)
                    {
                        
                        var toUser = item.ToUser;
                        using(var db2 = new CustomDbContext())
                        {
                            var users = db2.ProfileModel.Where(x => x.UserName == toUser).ToList();
                            foreach (var u in users)
                            {
                                var contains = false;
                                foreach (var i in modelList)
	                            {
		                            if(i.UserName == u.UserName)
                                    {
                                        contains = true;
                                        break;
                                    }

	                            }
                                if (!contains)
                                    modelList.Add(u);
                            }
                        }
                        
                    }
                    list = db.ChatMessages.Where(x => x.ToUser == s.user).ToList();
                    foreach (var item in list)
                    {

                        var fromUser = item.FromUser;
                        using (var db2 = new CustomDbContext())
                        {
                            var users = db2.ProfileModel.Where(x => x.UserName == fromUser).ToList();
                            foreach (var u in users)
                            {
                                var contains = false;
                                foreach (var i in modelList)
                                {
                                    if (i.UserName == u.UserName)
                                    {
                                        contains = true;
                                        break;
                                    }

                                }
                                if (!contains)
                                    modelList.Add(u);
                            }
                        }

                    }
                }
            }
            return PartialView("GetMentorList", modelList);
        }

        public ProfileModel getMentorInf(string user)
        {
            ProfileModel model = new ProfileModel();
            
            using (CustomDbContext db = new CustomDbContext())
            {
                var ment = db.ProfileModel.SingleOrDefault(x => x.UserName == user);
                if (ment != null && ment.UserName != null)
                {
                    model.Name = ment.Name;
                    model.UserName = ment.UserName;
                    model.UserPhoto = ment.UserPhoto;
                    model.About_me = ment.About_me;
                    model.MyTegs = ment.MyTegs;
                    model.Rate = ment.Rate;

                }
            }
            return model;
        }      


        public ActionResult MentorProfile(ProfileModel model)
        {            
            if (s.user != null && s.user != "")
            {
                model = getMentorInf(s.user);
            }
            return View("MentorProfile", model);
        }

        [HttpGet]
        public ActionResult UploadPhoto()
        {
            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                if (s.user != null && s.user != "")
                {

                    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == s.user);
                    if (user != null)
                    {
                        model = db.ProfileModel.SingleOrDefault(x => x.UserName == s.user);
                        db.SaveChanges();
                    }
                    else ModelState.AddModelError("Error", "Error");
                }
            }
            return Content(Convert.ToBase64String(model.UserPhoto));
            
        }        

        public ActionResult SetMentorRate(ProfileModel model)
        {
            string currentMent = s.user;
            if (currentMent != null && currentMent != "")                
            {
                using (CustomDbContext db = new CustomDbContext())
                {
                    var ment = db.ProfileModel.SingleOrDefault(x => x.UserName == currentMent);
                    if (ment != null)
                        ment.Rate = Convert.ToInt32(model.SelectedTeg.Last());
                    db.SaveChanges();
                }
                List<SelectListItem> listSelectListItems = new List<SelectListItem>();

                for (int i = 0; i < 5; i++)
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = (i + 1).ToString(),
                        Value = (i + 1).ToString()
                    };
                    listSelectListItems.Add(selectList);
                }
                model.TegList = listSelectListItems;


                using (CustomDbContext db = new CustomDbContext())
                {
                    var ment = db.ProfileModel.SingleOrDefault(x => x.UserName == currentMent);
                    if (ment != null && ment.UserName != null)
                    {
                        model.Name = ment.Name;
                        model.UserName = ment.UserName;
                        model.UserPhoto = ment.UserPhoto;
                        model.About_me = ment.About_me;
                        model.MyTegs = ment.MyTegs;
                        model.Rate = ment.Rate;
                    }
                }

            }
            return View("UserPreview", model);
        }           

    }
}
