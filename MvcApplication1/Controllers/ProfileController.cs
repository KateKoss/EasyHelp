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
        //Singleton s;
        [HttpGet]
        public ActionResult Index(ProfileModel model, string user)
        {
            if (user != null && user != "")
            {
                // Создать объект cookie-набора
                HttpCookie cookie = new HttpCookie("MentorId");
                // Установить значения в нем
                cookie.Value = user;
                // Добавить куки в ответ
                Response.Cookies.Add(cookie);

                model = getMentorInf(user);

                return View("MentorPage", model);
            }
            else
            {
                //s = Singleton.Instance;
                
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else {
                    Console.WriteLine("Can not read currentPerson from cookie.");
                    currentPerson = null;
                }//currentPerson = "user1";
                
                //s.user = currentPerson;
               
                using (CustomDbContext db = new CustomDbContext())
                {

                    var user1 = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                    if (user1 != null)
                    {
                        if (user1.Role == "mentor")
                        {
                            return RedirectToAction("MentorProfile");
                        }
                        var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                        if (myModel.UserPhoto == null)
                        {

                        }
                        var list = new List<String>();
                        if (myModel.MyTegs != null)
                        {
                            var splitTegs = myModel.MyTegs.Split(' ');
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

                MentorsModel mentor;
                //пошук ментора
                if (model.searchMentor != null && model.searchMentor != "")
                {
                    using (CustomDbContext db = new CustomDbContext())
                    {
                        string currentMentor;
                        var mentorList = db.UserProfiles.Where(x => x.Role == "mentor");
                        if (mentorList != null)
                            foreach (var m in mentorList)
                            {
                                currentMentor = m.UserName;
                                using (CustomDbContext db2 = new CustomDbContext())
                                {
                                    var ment = db2.ProfileModel.SingleOrDefault(x => x.UserName == currentMentor);
                                    if (ment != null && ment.Name != null)
                                        if (ment.Name.Contains(model.searchMentor))
                                        {
                                            mentor = new MentorsModel();
                                            mentor.Name = ment.Name;
                                            mentor.UserName = ment.UserName;
                                            mentor.UserPhoto = ment.UserPhoto;
                                            model.mentors.Add(mentor);
                                        }
                                }
                            }
                    }
                }//якщо пошук пустий - то підвантажуємо список менторів які мають такий самий тег
                else using (CustomDbContext db = new CustomDbContext())
                    {
                        if (db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson).MyTegs != null)
                        {
                            string[] currentPersonTegs = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson).MyTegs.Split(' ');

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
                                                    if (ment.UserName != currentPerson)
                                                        if (ment.MyTegs != null)
                                                            if (ment.MyTegs.Contains(currentTeg))
                                                                if (!mentorListWithTegs.Any(x => x.UserName == ment.UserName))
                                                                    mentorListWithTegs.Add(ment);
                                            }
                                        }
                                    }


                            foreach (var m in mentorListWithTegs)
                            {
                                mentor = new MentorsModel();
                                mentor.Name = m.Name;
                                mentor.UserName = m.UserName;
                                mentor.UserPhoto = m.UserPhoto;
                                model.mentors.Add(mentor);
                            }
                        }
                    }
                return View("Index", model);
            }
            //return View();
        }

        public ProfileModel getMentorInf(string user)
        {
            ProfileModel model = new ProfileModel();
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
            var rate = "";
            if (model.SelectedTeg != null)
                foreach (string s in model.SelectedTeg)
                    rate = s;

            using (CustomDbContext db = new CustomDbContext())
            {

                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                var user1 = db.UserProfiles.SingleOrDefault(x => x.UserName == user);
                if (user1 != null)
                {
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (rate != "")
                        myModel.Rate = Convert.ToInt32(rate);
                    db.SaveChanges();

                }
                else model = new ProfileModel() { };
            }
            using (CustomDbContext db2 = new CustomDbContext())
            {
                var ment = db2.ProfileModel.SingleOrDefault(x => x.UserName == user);
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

        [HttpPost]
        public ActionResult Index(ProfileModel model)
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";

                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    myModel.Name = model.Name;
                    myModel.About_me = model.About_me;
                    //myModel.MyTegs = model.MyTegs;

                    db.SaveChanges();
                }
                else ModelState.AddModelError("Error", "Error");
            }
            return RedirectToAction("Index");
        }

        public ActionResult MentorProfile(ProfileModel model)
        {
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            model = getMentorInf(currentPerson);
            return View("MentorProfile", model);
        }

        [HttpGet]
        public ActionResult Requests(ProfileModel model)
        {
            return View("Requests", model);
        }

        [HttpGet]
        public ActionResult ForProfileEditing()
        {
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "с++",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "с#",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "javascript",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            List<string> listTegItems = new List<string>();
            listTegItems.Add(selectList.Text);
            ProfileModel model;
            using (CustomDbContext db = new CustomDbContext())
            {

                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    model = new ProfileModel()
                    {
                        Name = myModel.Name,
                        MyTegs = myModel.MyTegs,
                        About_me = myModel.About_me,
                        TegList = listSelectListItems
                    };
                }
                else model = new ProfileModel() { };
            }
            return View("ForProfileEditing", model);
        }
        [HttpPost]
        public ActionResult ForProfileEditing(ProfileModel model)
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddTeg()
        {
            return View("ForProfileEditing");
        }
        [HttpPost]
        public ActionResult AddTeg(ProfileModel model)
        {
            /////////////потом создать таблицу в бд и внести данные туда
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "с++",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "с#",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "javascript",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            List<string> listTegItems = new List<string>();
            listTegItems.Add(selectList.Text);
            model.TegList = listSelectListItems;
            //////////////////////////

            Command.User u = new Command.User();
            u.AddTeg(model);

            //var str = "";
            //if (model.SelectedTeg != null)
            //    foreach (string s in model.SelectedTeg)
            //    {
            //        //model.TegList.SingleOrDefault(x => x.Value == s);

            //        str += model.TegList.SingleOrDefault(x => x.Value == s).Text;
            //    }

            //using (CustomDbContext db = new CustomDbContext())
            //{

            //    string currentPerson;
            //    if (Request.Cookies["UserId"] != null)
            //        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            //    else currentPerson = "user1";
            //    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
            //    if (user != null)
            //    {
            //        var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
            //        var tegs = myModel.MyTegs;
            //        if (tegs != null)
            //        {
            //            if (tegs.Contains(' ' + str + ' '))
            //                myModel.MyTegs = tegs.Replace(' ' + str + ' ', " ");
            //            else
            //            {
            //                if (tegs.Contains(' ' + str))
            //                    myModel.MyTegs = tegs.Replace(' ' + str, " ");
            //                else if (tegs.Contains(str + ' '))
            //                    myModel.MyTegs = tegs.Substring(str.Length+1);
            //                else myModel.MyTegs = tegs + str + " ";
            //            }                       
            //        }
            //        else myModel.MyTegs = str + " ";


            //        db.SaveChanges();
            //    }
            //    else model = new ProfileModel() { };
            //}


            return RedirectToAction("ForProfileEditing");
        }


        public ActionResult UploadPhoto()
        {
            ProfileModel model = new ProfileModel() { };

            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";

                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                    model = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    //myModel.UserPhoto = imageData;
                    //model = myModel;

                    db.SaveChanges();
                }
                else ModelState.AddModelError("Error", "Error");
            }
            return Content(Convert.ToBase64String(model.UserPhoto));
            
        }

        //[HttpPost]
        ////public ActionResult UploadPhoto()
        //public ActionResult UploadPhoto(ProfileModel model, HttpPostedFileBase upload)
        //{
        //    if (ModelState.IsValid && upload != null)
        //    {
        //        // получаем имя файла
        //        string fileName = System.IO.Path.GetFileName(upload.FileName);
        //        // сохраняем файл в папку Files в проекте
        //        upload.SaveAs(Server.MapPath("~/Files/" + fileName));

        //        byte[] imageData = null;
        //        // считываем переданный файл в массив байтов
        //        using (var binaryReader = new BinaryReader(upload.InputStream))
        //        {
        //            imageData = binaryReader.ReadBytes(upload.ContentLength);
        //        }
        //        model.UserPhoto = imageData;

        //        using (CustomDbContext db = new CustomDbContext())
        //        {
        //            string currentPerson;
        //            if (Request.Cookies["UserId"] != null)
        //                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
        //            else currentPerson = "user1";

        //            var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
        //            if (user != null)
        //            {
        //                var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
        //                myModel.UserPhoto = imageData;
        //                model = myModel;

        //                db.SaveChanges();
        //            }
        //            else ModelState.AddModelError("Error", "Error");
        //        }
        //    }
        //    return RedirectToAction("Index");
            
        //}

        public ActionResult SetMentorRate(ProfileModel model)
        {
            string currentMent;
            if (Request.Cookies["MentorId"] != null)
                currentMent = Convert.ToString(Request.Cookies["MentorId"].Value);
            else currentMent = "";
            if (currentMent != "")
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


                using (CustomDbContext db2 = new CustomDbContext())
                {
                    var ment = db2.ProfileModel.SingleOrDefault(x => x.UserName == currentMent);
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
            return View("MentorPage", model);

        }
    }
}
