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

        public ActionResult Index(ProfileModel model)
        {
           // model.UserPhoto = upload.SaveAs(Server.MapPath("~/Files/" + fileName)); ;
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




                   //var id = Request.Cookies["UserId"].Value;
                   string currentPerson;
                   if (Request.Cookies["UserId"] != null)
                       currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                   else currentPerson = "Luda";
                    //var currentPerson = "fhdgsdfj";
                   int id;
                    //var user = db.UserProfiles.SingleOrDefault(x => x.UserName == currentPerson);
                    //if (user != null)
                    //{
                    //    //id = user;
                    //    using (ProfileContext dbP = new ProfileContext())
                    //    {
                    //        dbP.ProfileDb.Add(model);
                    //        //dbP.ProfileDb.Add(new ProfileModel { About_me = model.About_me });
                    //        //dbP.ProfileDb.Add(new ProfileModel { Name = model.Name });
                    //        //dbP.ProfileDb.Add(new ProfileModel { UserPhoto = model.UserPhoto});
                    //        //dbP.ProfileDb.Add(new ProfileModel { MyTegs = model.MyTegs});
                            
                            
                    //        //user.About_me = model.About_me;
                    //        dbP.SaveChanges();
                    //    }
                    //}
                    //else ModelState.AddModelError("Error", "Error");
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
            ProfileModel myModel = new ProfileModel()
            {
                TegList = listSelectListItems
                //SelectedTeg = listTegItems

            };
            return View("ForProfileEditing",myModel);
        }
        [HttpPost]
        public ActionResult ForProfileEditing(ProfileModel model)
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
            //ProfileModel myModel = new ProfileModel()
            //{
            //    TegList = listSelectListItems
            //    //SelectedTeg = listTegItems

            //};
            StringBuilder sb = new StringBuilder();
            ProfileModel myModel;

            var str="";
            if(model.SelectedTeg != null)
            foreach (string s in model.SelectedTeg)
               str += "fjf    " + s;
           
            
                myModel = new ProfileModel()
                {
                    TegList = listSelectListItems
                    //MyTegs = listSelectListItems[model.SelectedTeg];
                    //MyTegs = sb.Append(string.Join(" ", selectedTegs)).ToString()
                    //SelectedTeg = listTegItems
                };
            
            model.MyTegs = " dfjjff";
            //model.SelectedTeg = model.SelectedTeg;
                myModel = new ProfileModel()
                {
                    TegList = listSelectListItems,
                    MyTegs = str
                    //SelectedTeg = listTegItems
                };
            
            return View("ForProfileEditing", myModel);
        }
        [HttpPost]
        public ActionResult AddTeg(ProfileModel model, LoginModel lm)
        {
            return View("Index", model);
        }
        //[HttpPost]
        //public ActionResult AddTeg()
        //{
        //    StringBuilder sb = new StringBuilder();
            
        //    ProfileModel myModel = new ProfileModel()
        //    {
                
        //         //MyTegs = sb.Append(string.Join(" ", selectedTegs)).ToString()
        //        //SelectedTeg = listTegItems

        //    };
        //    return View( myModel);
        //}

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
            }
            return View("Index", model);
        }

    }
}
