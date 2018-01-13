using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.Contexts;
using System.IO;

namespace MvcApplication1.Controllers
{
    public class EditController : Controller
    {
        Singleton s = Singleton.Instance;

        public class Data
        {
            public string username { get; set; }
            public string AboutMe { get; set; }
            public string addTeg { get; set; }
            public string removeTeg { get; set; }
            
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditName(Data data)
        {

            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {

                string currentPerson = s.user;
                if (currentPerson != null && data.username != null)
                {
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model!= null)
                    {
                        if (data.username != null)
                        {
                            if (data.username != model.Name)
                            {
                                model.Name = data.username;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            model.Name = "";
                            db.SaveChanges();
                        }
                    }

                }
            }
            return Content(model.Name);
        }

        public ActionResult EditPhoto(Data data)
        {
            byte[] buffer = new byte[0];
            ProfileModel model = new ProfileModel() { };
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var file = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                MemoryStream ms = new MemoryStream();
                //var bytes = System.IO.File.ReadAllBytes(pic);
                buffer = new byte[file.ContentLength];
                file.InputStream.Read(buffer, 0, file.ContentLength);
              

                using (CustomDbContext db = new CustomDbContext())
                {
                    string currentPerson = s.user;
                    if (currentPerson != null)
                    {
                        var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                        if (user != null)
                        {
                            var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                            myModel.UserPhoto = buffer;
                            model = myModel;

                            db.SaveChanges();
                        }
                        else ModelState.AddModelError("Error", "Error");
                    }
                }
                //model.UserPhoto = buffer;
            }


            return Content(Convert.ToBase64String(buffer, Base64FormattingOptions.None));
        }

        public ActionResult EditAboutMe(Data data)
        {

            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson = s.user;
                if (currentPerson != null)
                {                    
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (data.AboutMe != null)
                        {
                            if (data.AboutMe != model.About_me)
                            {
                                model.About_me = data.AboutMe;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            model.About_me = "";
                            db.SaveChanges();
                        }
                    }
                    
                }
            }
            return Content(model.About_me);
        }

        //POST
        public ActionResult Tegs(Data data)
        {
            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson = s.user;
                if (currentPerson != null)
                {
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (model.Tegs != null && data.addTeg != null)
                        {
                            bool tagExists = false;
                            var tags = model.Tegs.Split('|');
                            foreach (var tag in tags)
	                        {
		                        if (tag == data.addTeg && tag !="")
                                {
                                    tagExists = true;
                                    break;
                                }                                       
                                    
	                        }

                            if (!tagExists)
                            {
                                model.Tegs += data.addTeg + '|';
                                db.SaveChanges();
                            }     
                        }
                        if ((model.Tegs == null || model.Tegs == "") && data.addTeg != null)
                        {
                            model.Tegs += data.addTeg + '|';
                            db.SaveChanges();
                            model.tegs.Add(data.addTeg);
                        } 
                        else
                        {
                            var splitTegs = model.Tegs.Split('|');
                            foreach (var el in splitTegs)
                            {
                                if (el != "")
                                    model.tegs.Add(el);
                            }
                        }
                    }
                }
            }        

            return PartialView(model);
        }

        //GET
        [HttpGet]
        public ActionResult Tegs()
        {
            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson = s.user;
                if (currentPerson != null)
                {
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (model.Tegs != null)
                        {
                            var splitTegs = model.Tegs.Split('|');
                            foreach (var el in splitTegs)
                            {
                                if (el != "")
                                    model.tegs.Add(el);
                            }
                        }

                    }

                }
            }           

            return PartialView(model);
        }

        public ActionResult RemoveTegs(Data data)
        {
            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson = s.user;
                if (currentPerson != null && data.removeTeg != null)
                {
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (model.Tegs != null)
                        {
                            var tags = model.Tegs.Split('|');

                            for(int i=0; i<tags.Length-1; i++)
                            {
                                if (tags[i]==data.removeTeg && tags[i] != "" && data.removeTeg !="")
                                {
                                    Array.Clear(tags, i, 1);
                                }
                            }
                            model.Tegs = "";
                            for (int i = 0; i < tags.Length - 1; i++)
                            {
                                if (tags[i]!= null)
                                    model.Tegs += tags[i]+'|';
                            }
                            
                            db.SaveChanges();      

                            var splitTegs = model.Tegs.Split('|');
                            foreach (var el in splitTegs)
                            {
                                if (el != "")
                                    model.tegs.Add(el);
                            }
                        }
                    }
                }
            }

            return PartialView("Tegs", model);
        }
    }
}
