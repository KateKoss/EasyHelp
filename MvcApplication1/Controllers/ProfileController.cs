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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
    public class ProfileController : Controller
    {
        [HttpGet]
        public ActionResult Index(ProfileModel model)
        {

            
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            
            using (CustomDbContext db = new CustomDbContext())
            {
                
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
                        UserPhoto = myModel.UserPhoto,
                        searchMentor = model.searchMentor
                    };
                }
                else model = new ProfileModel() { };
            }

            MentorsModel mentor = new MentorsModel();
            
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
                                        mentor.Name = ment.Name;
                                        mentor.UserName = ment.UserName;
                                        mentor.UserPhoto = ment.UserPhoto;
                                        model.mentors.Add(mentor);
                                    }
                            }
                        }
                }
            } else

            using (CustomDbContext db = new CustomDbContext())
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
                                                        if (!mentorListWithTegs.Contains(ment))
                                                            mentorListWithTegs.Add(ment);
                                    }
                                }
                            }


                    foreach (var m in mentorListWithTegs)
                    {
                        mentor.Name = m.Name;
                        mentor.UserName = m.UserName;
                        mentor.UserPhoto = m.UserPhoto;
                        model.mentors.Add(mentor);
                    }
                }                
            }
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult Index(ProfileModel model, LoginModel lm)
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

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "saveToDraft")]
        public ActionResult SaveArticleToDraft(ArticleModel model)
        {
            if (model.articleTitle != null)
                using (CustomDbContext db = new CustomDbContext())
                {
                    string articleid;
                    string currentPerson;
                    if (Request.Cookies["UserId"] != null)
                        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                    else currentPerson = "user1";

                    //чи створював користувач вже статті
                    var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson);
                    if (myModel == null)
                    {
                        articleid = currentPerson + '_' + 1;
                        //якщо ні, то id статті 1, бо ця стаття в нього перша
                        db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                        if (myModel != null)//якщо є
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = false;
                        }
                        db.SaveChanges();  
                    
                    } else
                    //якщо користувач обрав існуючу статтю для редагування
                    if (model.articleID != null)
                    {
                        articleid = currentPerson + "_" + model.articleID;
                        //перевіряємо чи власне є ця стаття у бд
                        myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleid);
                        if (myModel!=null)//якщо є
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = false;
                            db.SaveChanges();  
                        }
                        else
                        {

                            db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                            myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleid);
                            if (myModel != null)
                            {
                                myModel.articleTitle = model.articleTitle;
                                myModel.articleText = model.articleText;
                                myModel.isPublished = false;
                            }
                            db.SaveChanges();  
                        }
                    }
                    else //якщо користувач створює нову статтю, проте в нього були вже створені статті
                    {
                        var articleCount = db.ArticleModel.Count(x => x.createdBy == currentPerson);
                        articleid = currentPerson + '_' + (articleCount + 1);
                        db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                        myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleid);
                        if (myModel != null)
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = false;
                        }
                        db.SaveChanges();  
                    }
                
                                
                }

            //сохранить статью в бд
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "publish")]
        public ActionResult PublishArticle(ArticleModel model)
        {
            if (model.articleTitle!=null)
                using (CustomDbContext db = new CustomDbContext())
                {
                    string articleid;
                    string currentPerson;
                    if (Request.Cookies["UserId"] != null)
                        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                    else currentPerson = "user1";

                    //чи створював користувач вже статті
                    var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson);
                    if (myModel == null)
                    {
                        articleid = currentPerson + '_' + 1;
                        //якщо ні, то id статті 1, бо ця стаття в нього перша
                        db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                        myModel = db.ArticleModel.SingleOrDefault(x => x.articleID == articleid);
                        if (myModel != null)
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = true;
                        }
                    
                    } else
                    //якщо користувач обрав існуючу статтю для редагування
                    if (model.articleID != null)
                    {
                        articleid = currentPerson + "_" + model.articleID;
                        //перевіряємо чи власне є ця стаття у бд
                        myModel = db.ArticleModel.SingleOrDefault(x => x.articleID == articleid);
                        if (myModel != null)//якщо є
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = true;
                        }
                        else
                        {
                            db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                            myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleid);
                            if (myModel != null)
                            {
                                myModel.articleTitle = model.articleTitle;
                                myModel.articleText = model.articleText;
                                myModel.isPublished = true;
                            }

                        }
                    }
                    else //якщо користувач створює нову статтю, проте в нього були вже створені статті
                    {
                        var articleCount = db.ArticleModel.Count(x => x.createdBy == currentPerson);
                        articleid = currentPerson + '_' + (articleCount + 1);
                        db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                        myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleid);
                        if (myModel != null)
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = true;
                        }

                    }

                    db.SaveChanges();
                }
            
            //опубликовать статью
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MyArticle()
        {
            
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            //SelectListItem selectList = new SelectListItem()
            //{
            //    Text = "Взаємодія з даними",
            //    Value = "1"
            //};
            //listSelectListItems.Add(selectList);
            //selectList = new SelectListItem()
            //{
            //    Text = "Міграція даних",
            //    Value = "2"
            //};
            
            //listSelectListItems.Add(selectList);
            //selectList = new SelectListItem()
            //{
            //    Text = "Принцип SOLID",
            //    Value = "3"
            //};
            //listSelectListItems.Add(selectList);
            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                
                foreach (var article in db.ArticleModel.Where(x => x.createdBy == currentPerson)) 
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = article.articleTitle,
                        Value = article.articleID
                    };
                    listSelectListItems.Add(selectList);
                }
            }
            ArticleModel myModel = new ArticleModel()
            {
                articleNames = listSelectListItems
            };
            return View(myModel);
        }

        [HttpGet]
        public ActionResult CreateArticle(ArticleModel model)
        {
            return View("CreateArticle", model);
        }

        [HttpGet]
        public ActionResult EditArticle(ArticleModel model)
        {
            return View("CreateArticle", model);
        }

        [HttpPost]
        public ActionResult EditArticle(ArticleModel model, LoginModel lm)
        {
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
                        
            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";

                foreach (var article in db.ArticleModel.Where(x => x.createdBy == currentPerson))
                {
                    SelectListItem selectList = new SelectListItem()
                    {
                        Text = article.articleTitle,
                        Value = article.articleID
                    };
                    listSelectListItems.Add(selectList);
                }
            }
            model.articleNames = listSelectListItems;
            var str = "";
            if (model.articleName != null)
                foreach (string s in model.articleName)
                {
                    var str1 = model.articleNames.FirstOrDefault(x => x.Value == s).Text;
                    str += str1;
                    model.articleID = s;
                }

            using (CustomDbContext db = new CustomDbContext())
            {

                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                
                var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson);
                if (myModel != null)
                { 
                    
                    model.articleTitle = str;
                    if (str!="")
                        model.articleText = myModel.articleText;
                    model.articleNames = listSelectListItems;
                    
                }
                
            }
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

            var str = "";
            if (model.SelectedTeg != null)
                foreach (string s in model.SelectedTeg)
                {
                    //model.TegList.SingleOrDefault(x => x.Value == s);

                    str += model.TegList.SingleOrDefault(x => x.Value == s).Text + " ";
                }

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
                    var tegs = myModel.MyTegs;
                    if (tegs != null)
                        if (tegs.Contains(str))
                            myModel.MyTegs = tegs.Replace(str, " ");
                        else myModel.MyTegs = tegs + str;
                    else myModel.MyTegs = str;


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
                
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(upload.InputStream))
                {
                    imageData = binaryReader.ReadBytes(upload.ContentLength);
                }
                model.UserPhoto = imageData;

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
