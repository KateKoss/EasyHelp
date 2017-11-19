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
            
        }

        public class Data
        {
            public string rate { get; set; }
            public string name { get; set; }
        }
        
        [HttpGet]
        public ActionResult Index(ProfileModel model)
        {
            if (Request != null)
                if (Request.Cookies["UserId"] != null)
                    s.user = Convert.ToString(Request.Cookies["UserId"].Value);
            if (s.user != null && s.user != "")
            {
                using (CustomDbContext db = new CustomDbContext())
                {

                    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == s.user);
                    if (user != null)
                    {
                        var infoAboutUser = db.ProfileModel.SingleOrDefault(x => x.UserName == s.user);

                        var list = new List<String>();
                        if (infoAboutUser.Tegs != null)
                        {
                            var splitTegs = infoAboutUser.Tegs.Split('|');

                            foreach (var el in splitTegs)
                            {
                                if (el !="")
                                    list.Add(el);

                            }
                        }
                        model = new ProfileModel()
                        {
                            Name = infoAboutUser.Name,
                            Tegs = infoAboutUser.Tegs,
                            About_me = infoAboutUser.About_me,
                            UserPhoto = infoAboutUser.UserPhoto,
                            searchMentor = model.searchMentor,
                            Rate = infoAboutUser.Rate,
                            tegs = list,
                            isMentor = user.Role=="mentor"?true:false
                        };
                    }
                    else model = new ProfileModel() { };
                }
            }
            return View("Index", model);
            
        }

        [HttpGet]
        public ActionResult UserPreview(string user)
        {
            ProfileModel model = new ProfileModel() { };
            if (user != null && user != "")
            {
                using (CustomDbContext db = new CustomDbContext())
                {

                    var userFromDb = db.UserProfiles.SingleOrDefault(x => x.UserName == user);
                    if (userFromDb != null)
                    {
                        var infoAboutUser = db.ProfileModel.SingleOrDefault(x => x.UserName == user);
                        
                        var list = new List<String>();
                        if (infoAboutUser.Tegs != null)
                        {
                            var splitTegs = infoAboutUser.Tegs.Split('|');

                            foreach (var el in splitTegs)
                            {
                                if(el != "")
                                    list.Add(el);

                            }
                        }
                        model = new ProfileModel()
                        {
                            Name = infoAboutUser.Name,
                            Tegs = infoAboutUser.Tegs,
                            About_me = infoAboutUser.About_me,
                            UserPhoto = infoAboutUser.UserPhoto,
                            Rate = infoAboutUser.Rate,
                            searchMentor = model.searchMentor,
                            tegs = list,
                            isMentor = userFromDb.Role == "mentor" ? true : false
                        };
                    }
                    else model = new ProfileModel() { };
                }
            }
            if (model != null)
                return View("UserPreview", model);
            else return Content("error");
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
                    if (db.ProfileModel.SingleOrDefault(x => x.UserName == s.user).Tegs != null)
                    {
                        string[] currentPersonTegs = db.ProfileModel.SingleOrDefault(x => x.UserName == s.user).Tegs.Split('|');

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
                                                    if (ment.Tegs != null)
                                                    {
                                                        var tags = ment.Tegs.Split('|');

                                                        foreach (var tag in tags)
                                                        {
                                                            if(tag != "" && tag==currentTeg)
                                                                if (!mentorListWithTegs.Any(x => x.UserName == ment.UserName))
                                                                    mentorListWithTegs.Add(ment);
                                                        }       
                                                    }
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
            if (model != null)
            {
                if (model.UserPhoto != null)
                    return Content(Convert.ToBase64String(model.UserPhoto));
                else return Content("error");
            }
            else return Content("error");
            
        }        
        
        public ActionResult SetMentorRate(Data data)
        {

            string currentMent = s.user;
            ProfileModel ment = new ProfileModel() { };

            if (currentMent != null && currentMent != "")
            {
                using (CustomDbContext db = new CustomDbContext())
                {
                    ment = db.ProfileModel.SingleOrDefault(x => x.UserName == data.name);
                    if (ment != null)
                    {
                        if (ment.Rates != null)
                        {
                            var rates = ment.Rates.Split('|');
                            int MentorRate = 0;
                            foreach (var rate in rates)
                            {
                                MentorRate += Convert.ToInt32(rate);
                            }
                            data.rate = (Convert.ToInt32(data.rate) + 1).ToString();
                            MentorRate += Convert.ToInt32(data.rate);
                            ment.Rate = MentorRate / (rates.Length + 1);
                            ment.Rates += data.rate + "|";
                            ment.WhoRates += currentMent + "|";
                        }
                    }
                    db.SaveChanges();                    
                }           
            }
            return Content(ment.Rate.ToString());
        }           

    }
}
