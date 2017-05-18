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
                        About_me = myModel.About_me
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
                int id;
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
                        About_me = myModel.About_me
                    };
                }
                else model = new ProfileModel() { };
            }
            return View("ForProfileEditing", model);
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
                    MyTegs = str,
                    Name ="kate"
                    //SelectedTeg = listTegItems
                };

                return RedirectToAction("AddTeg");
        }
        [HttpGet]
        public ActionResult AddTeg(ProfileModel model, LoginModel lm)
        {
           
            //using (ProfileContext db = new ProfileContext())
            //{
            //    var currentPerson = "Luda";
            //    var user = db.ProfileDb.SingleOrDefault(x => x.UserName == currentPerson);
            //    if (user != null)
                   
            //    model = db.ProfileDb.Where(x => x.UserName == currentPerson).SingleOrDefault();

            //    //model.Name = "dd";
            //}
            return View("ForProfileEditing", model);
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
