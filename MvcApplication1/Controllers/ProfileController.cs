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


namespace MvcApplication1.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [HttpGet]
        public ActionResult Index()
        {
            ProfileModel model;
            //var s = Server.MapPath("/Images/short.jpg");
           // model.UserPhoto = upload.SaveAs(Server.MapPath("~/Files/" + fileName)); ;
            using (CustomDbContext db = new CustomDbContext())
            {
                
                var currentPerson = "user1";
                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    if (myModel.UserPhoto == null)
                    {

                    }
                    model = new ProfileModel()
                    {
                        Name = myModel.Name,
                        MyTegs = myModel.MyTegs,
                        About_me = myModel.About_me,
                        UserPhoto = myModel.UserPhoto
                    };
                }
                else model = new ProfileModel(){ };
            }            

            return View("Index", model);
        }
        
        
        [HttpPost]
        public ActionResult Index(ProfileModel model, LoginModel lm )
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                //var id = Request.Cookies["UserId"].Value;
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                //var currentPerson = "fhdgsdfj";
               
                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                       
                    model.UserName = "user1";
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    myModel.Name = model.Name;
                    myModel.About_me = model.About_me;
                    myModel.MyTegs = model.MyTegs;
                    //dbP.ProfileModel.Add(model);
                    //dbP.ProfileDb.Add(new ProfileModel { About_me = model.About_me });
                    //dbP.ProfileDb.Add(new ProfileModel { Name = model.Name });
                    //dbP.ProfileDb.Add(new ProfileModel { UserPhoto = model.UserPhoto});
                    //dbP.ProfileDb.Add(new ProfileModel { MyTegs = model.MyTegs});

                    db.SaveChanges();
                      
                }
                else ModelState.AddModelError("Error", "Error");
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult saveArticleToDraft(ProfileModel model, LoginModel lm)
        {
            //сохранить статью в бд
            return View("Index", model);
        }

        public ActionResult publishArticle(ProfileModel model, LoginModel lm)
        {
            //опубликовать статью
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult MyArticle(ProfileModel model)
        {
            UsersContext db = new UsersContext();
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "Взаємодія з даними",
                Value = "1"
            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "Міграція даних",
                Value = "2"
            };
            selectList = new SelectListItem()
            {
                Text = "Взаємодія з даними",
                Value = "1"
            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "Принцип SOLID",
                Value = "3"
            };
            listSelectListItems.Add(selectList);
            //foreach (var article in db.UserProfiles) //тут я не знаю
            //{
            //    SelectListItem selectList = new SelectListItem()
            //    {
            //        Text = article.Name,
            //        Value = article.ID
            //    };
            //    listSelectListItems.Add(selectList);
            //}

            ProfileModel myModel = new ProfileModel()
            {
                articleNames = listSelectListItems
            };
            return View(myModel);
        }

        [HttpGet]
        public ActionResult CreateArticle(ProfileModel model)
        {
            return View("CreateArticle", model);
        }

        [HttpGet]
        public ActionResult FindAssosiates(ProfileModel model)
        {
            return View("FindAssosiates", model);
        }

        [HttpPost]
        public ActionResult RetakeTest(ProfileModel model)
        {
            //пройти тест заново
            return View("FindAssosiates", model);
        }

        public ActionResult ConfirmTestTest(ProfileModel model)
        {
            //пройти тест заново
            return View("Index", model);
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

                var currentPerson = "user1";
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

            var str = "";
            if (model.SelectedTeg != null)
                foreach (string s in model.SelectedTeg)
                {
                    //model.TegList.SingleOrDefault(x => x.Value == s);

                    str += model.TegList.SingleOrDefault(x => x.Value == s).Text + " ";
                }

            using (CustomDbContext db = new CustomDbContext())
            {

                var currentPerson = "user1";
                var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                if (user != null)
                {
                    var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                    var tegs = myModel.MyTegs;
                    if (tegs.Contains(str))
                        myModel.MyTegs = tegs.Replace(str, " ");
                    else myModel.MyTegs = tegs + str;
                    
                    
                    db.SaveChanges();
                }
                else model = new ProfileModel() { };
            }


            return RedirectToAction("ForProfileEditing");
        }

        
        public ActionResult UploadPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPhoto(ProfileModel model, HttpPostedFileBase upload)
        {
            
            if (ModelState.IsValid && upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                ////считаем загруженный файл в массив
                //byte[] avatar = new byte[upload.ContentLength];
                //upload.InputStream.Read(avatar, 0, upload.ContentLength);
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(upload.InputStream))
                {
                    imageData = binaryReader.ReadBytes(upload.ContentLength);
                }
                // установка массива байтов
                
                model.UserPhoto = imageData;

                using (CustomDbContext db = new CustomDbContext())
                {
                    //var id = Request.Cookies["UserId"].Value;
                    string currentPerson;
                    if (Request.Cookies["UserId"] != null)
                        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                    else currentPerson = "user1";
                    //var currentPerson = "fhdgsdfj";

                    var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                    if (user != null)
                    {

                        //model.UserName = "user1";
                        var myModel = db.ProfileModel.SingleOrDefault(x => x.UserName == currentPerson);
                        myModel.UserPhoto = imageData;
                        model = myModel;

                        db.SaveChanges();

                    }
                    else ModelState.AddModelError("Error", "Error");
                }
            }
            return View("Index", model);
        }

    }
}
