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
        //
        // GET: /Edit/
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

                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                {
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);

                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model!= null)
                    {                        
                        if (data.username != null)
                            if (data.username != model.Name)
                            {
                                model.Name = data.username;
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
                    string currentPerson;
                    if (Request.Cookies["UserId"] != null)
                    {
                        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                        //else currentPerson = "user1";

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

                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                {
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);

                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {                        
                        if (data.AboutMe != null)
                            if (data.AboutMe != model.About_me)
                            {
                                model.About_me = data.AboutMe;
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
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                {
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);

                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (data.addTeg != null)
                        {
                            if (data.addTeg.Contains(' '))
                            {
                                data.addTeg.Replace(' ', '_');
                            }
                            if (!model.MyTegs.Contains(data.addTeg))
                            {
                                string tegs = model.MyTegs;
                                model.MyTegs = tegs + ' ' + data.addTeg;
                                db.SaveChanges();
                                
                            }
                        }

                        var splitTegs = model.MyTegs.Split(' ');
                        foreach (var el in splitTegs)
                        {
                            model.tegs.Add(el);
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
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                {
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);

                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        var splitTegs = model.MyTegs.Split(' ');
                        foreach (var el in splitTegs)
                        {
                            model.tegs.Add(el);
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
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                {
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);

                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (model != null)
                    {
                        if (data.removeTeg != null)
                        {
                            if (data.removeTeg.Contains(' '))
                            {
                                data.removeTeg.Replace(' ', '_');
                            }
                            if (model.MyTegs.Contains(' ' + data.removeTeg + ' '))
                            {
                                string tegs = model.MyTegs;
                                model.MyTegs = tegs.Replace(' ' + data.removeTeg + ' ', " ");
                                db.SaveChanges();
                            }
                            else if (model.MyTegs.Contains(' ' + data.removeTeg))
                            {
                                string tegs = model.MyTegs;
                                model.MyTegs = tegs.Replace(' ' + data.removeTeg, " ");
                                db.SaveChanges();
                            }
                            else if (model.MyTegs.Contains(data.removeTeg + ' '))
                            {
                                string tegs = model.MyTegs;
                                model.MyTegs = tegs.Replace(data.removeTeg + ' ', " ");
                                db.SaveChanges();
                            }
                        }

                        var splitTegs = model.MyTegs.Split(' ');
                        foreach (var el in splitTegs)
                        {
                            model.tegs.Add(el);
                        }
                        

                    }

                }
            }

            return PartialView("Tegs", model);
        }
    }
}
