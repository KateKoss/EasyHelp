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


namespace MvcApplication1.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {

            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "с++",
                Value = "1"

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
            //List<string> listTegItems = new List<string>();
            //listTegItems.Add(selectList.Text);
            ProfileModel myModel = new ProfileModel()
            {
                TegList = listSelectListItems
                //SelectedTeg = listTegItems

            };
            
            //model.TegList = new List<string>();
            //model.TegList.Add("Vasia");
            //model.TegList.Add("Tania");
            //model.flag_about_me = !model.flag_about_me;
            return View(myModel);
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





                    //var currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                    //var currentPerson = "fhdgsdfj";
                    //var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                    //if (user != null)
                    //{
                    //    db.UserProfiles.Add(new UserProfile { About_me = model.About_me });
                    //    //user.About_me = model.About_me;
                    //    db.SaveChanges();
                    //}
                    //else ModelState.AddModelError(currentPerson, currentPerson + "Error");
                }
                //TODO: SubscribeUser(model.Email);
            }



            
            return View("Index", model);
        }
        //[HttpPost]
        //public string Index(IEnumerable<string> selectedCities)
        //{
        //    if (selectedCities == null)
        //    {
        //        return "No cities are selected";
        //    }
        //    else
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("You selected – " + string.Join(",", selectedCities));
        //        return sb.ToString();
        //    }
        //}
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
        public ActionResult ForProfileEditing(ProfileModel model)
        {
            return View("ForProfileEditing", model);
        }

        public ActionResult UploadPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPhoto(ProfileModel model, HttpPostedFileBase upload)
        {
            if (upload != null)
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
            }
            return View("Index", model);
        }

    }
}
