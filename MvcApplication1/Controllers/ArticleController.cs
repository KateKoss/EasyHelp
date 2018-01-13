using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.Contexts;
using System.Reflection;

namespace MvcApplication1.Controllers
{
    public class ArticleData
    {
        public string inputArticleTitle { get; set; }
        public string inputArticleText { get; set; }
        public bool isPublished { get; set; }
        public string[] tagList { get; set; }
    }

    public class ArticleController : Controller
    {
        public IEnumerable<ArticleModel> getArticles(string currentPerson, bool isPublsihed)
        {
            List<ArticleModel> listOfArticles = new List<ArticleModel>();
            using (CustomDbContext db = new CustomDbContext())
            {
                listOfArticles.AddRange(db.ArticleModel.Where(x => x.createdBy == currentPerson && x.isPublished == isPublsihed));
            }
            
            foreach(ArticleModel article in listOfArticles)
            {
                if (article.tagList != "" && article.tagList != null)
                {
                    article.chosenTags = new List<String>();            //для отображения тегов на страничке
                    article.chosenTags.AddRange(article.tagList.Split('|'));
                }
            }            
            return listOfArticles;
        }

        public ArticleModel getArticle(string currentPerson, string articleId)
        {
            ArticleModel article = new ArticleModel();
            using (CustomDbContext db = new CustomDbContext())
            {
                article = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson && x.articleID == articleId);
                if (article.tagList != null)
                {
                    article.chosenTags = new List<String>();            //для отображения тегов на страничке
                    article.chosenTags.AddRange(article.tagList.Split('|'));
                }                
                return article;
            }
        }

        // GET: Article
        [HttpGet]
        public ActionResult Index()
        {
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = null;
            //ProxyPattern.Proxy p = new ProxyPattern.Proxy(); //!!!!!!!!!!!!!!!!!!!!!!!
            //ArticleModel myModel = p.getArticle(currentPerson, false);
            
            IEnumerable<ArticleModel> myModel = getArticles(currentPerson, false);
            if (Request.IsAjaxRequest())
                return PartialView("_ArticlesPartial", myModel);
            return View("MentorArticle", myModel);
        }

        [HttpGet]
        public ActionResult EditArticle(ArticleModel model)
        {
            return View("CreateArticle", model);
        }


        public void LoadArtcileIntoForm(ArticleModel model, string currentPerson, bool onlyPublished)
        {
            //FacadePattern.Facade f = new FacadePattern.Facade(model, currentPerson, onlyPublished);
            //f.workWithSelectedTeg();
            //f.workWithDb();


            //model.articleNames = getArticle(currentPerson, onlyPublished).articleNames;
            //var str = "";
            //if (model.articleName != null)
            //    foreach (string s in model.articleName)
            //    {
            //        str = model.articleNames.FirstOrDefault(x => x.Value == s).Text;
            //        model.articleID = s;
            //    }

            //using (CustomDbContext db = new CustomDbContext())
            //{
            //    var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson);
            //    if (myModel != null)
            //    {
            //        model.articleTitle = str;
            //        if (str != "")
            //            model.articleText = myModel.articleText;
            //    }

            //}
            //return model;
        }

        [HttpPost]
        public ActionResult SaveArticle(ArticleData articleData)//ArticleModel model
        {
            //Decorator.SavaArticle d = new Decorator.SaveToDraft(new Decorator.ImplementSave());
            //d.saveToDb(model);
            //Singleton s = Singleton.Instance;
            string currentPerson;// = s.user;
            string tegString = "";
            
            if (articleData.tagList == null)
                return null;//make error page


            List<string> selectedTegs = articleData.tagList.ToList();
            for (int i = 0; i < selectedTegs.Count; i++)
            {                
                if (i != selectedTegs.Count - 1)
                {
                    tegString += selectedTegs[i] + "|";
                }
                else tegString += selectedTegs[i];
            }
            if (Request.Cookies["UserId"] != null) //если данные о пользователе записаны в куки
            {
                if (articleData.inputArticleTitle != "" && articleData.inputArticleText != "" && articleData.tagList != null)
                {
                    using (CustomDbContext db = new CustomDbContext())
                    {
                        string articleid;
                        currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                        Random rnd = new Random();
                        articleid = currentPerson + rnd.Next(0, 10).ToString();
                        for (int i = 0; i < 3; i++)
                        {
                            articleid += rnd.Next(0, 10).ToString();
                        }
                        var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson && x.articleID == articleid);//чи створював користувач вже статті
                        if (myModel == null) //если у пользователя не было статтей c таким id
                        {
                            db.ArticleModel.Add(new ArticleModel
                            {
                                articleID = articleid,
                                createdBy = currentPerson,
                                articleTitle = articleData.inputArticleTitle,
                                articleText = articleData.inputArticleText,
                                tagList = tegString,
                                dateOfCreation = DateTime.Now,
                                dateOfLastEditing = DateTime.Now,
                                isPublished = articleData.isPublished,
                                chosenTags = selectedTegs
                            });
                            try {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("{0} Exception caught.", e);
                            }
                        }                        
                    }
                }
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult PublishArticle(string articleId)//ArticleModel model
        {
            //Decorator.SavaArticle d = new Decorator.Publish(new Decorator.ImplementSave());
            //d.saveToDb(model);
            using (CustomDbContext db = new CustomDbContext())
            {
                var myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == articleId);
                myModel.isPublished = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult DeleteArticle(string articleId)
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                var modelToDelete = db.ArticleModel.FirstOrDefault(x => x.articleID == articleId);
                if (modelToDelete != null)
                {
                    db.ArticleModel.Remove(modelToDelete);
                    db.SaveChanges();
                }
                //TODO: else return error page
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult ShowArticles(string isPublished)
        {
            //string currentMent;
            //if (Request.Cookies["MentorId"] != null)
            //    currentMent = Convert.ToString(Request.Cookies["MentorId"].Value);
            //else currentMent = "";
            string currentPerson;// = s.user;
            IEnumerable<ArticleModel> myModel = null;
            if (Request.Cookies["UserId"] != null) //если данные о пользователе записаны в куки
            {
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                try
                {
                    myModel = getArticles(currentPerson, Convert.ToBoolean(isPublished));
                    if (Request.IsAjaxRequest())
                        return PartialView("_ArticlesPartial", myModel);
                    return View("MentorArticle", myModel);
                }
                catch
                {
                    return null; //TODO: return error page
                }
            }
            return View("MentorArticle", myModel);
            //if (currentMent != "")
            //{
            //    LoadArtcileIntoForm(model, currentMent, true);
            //    model.createdBy = currentMent;

            //    List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            //    SelectListItem selectList = new SelectListItem()
            //    {
            //        Text = "Подобається",
            //        Value = "1",
            //        Selected = true

            //    };
            //    listSelectListItems.Add(selectList);
            //    selectList = new SelectListItem()
            //    {
            //        Text = "Не подобається",
            //        Value = "2"

            //    };
            //    listSelectListItems.Add(selectList);

            //    model.TegList = listSelectListItems;
            //    LikeDislike(model);
            //}
            //return View("ShowArticles", model);
        }

        [HttpGet]
        public ActionResult ShowArticleDetails(string articleId)
        {
            string currentPerson;// = s.user;
            ArticleModel myModel = null;
            if (Request.Cookies["UserId"] != null) //если данные о пользователе записаны в куки
            {
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                myModel = getArticle(currentPerson, articleId);
                return View("ArticlePreview", myModel);
            }
            return View("ArticlePreview", myModel);
        }

        [HttpPost]
        public ActionResult EditArticle(ArticleModel model, ProfileModel p)
        {
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            LoadArtcileIntoForm(model, currentPerson, false);

            return View("CreateArticle", model);
        }

        public void LikeDislike(ArticleModel model)
        {
            string currentMent;
            if (Request.Cookies["MentorId"] != null)
                currentMent = Convert.ToString(Request.Cookies["MentorId"].Value);
            else currentMent = "";
            if (currentMent != "")
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";

                using (CustomDbContext db = new CustomDbContext())
                {
                    var m = db.ArticleModel.SingleOrDefault(x => x.articleID == model.articleID);
                    if (model.SelectedTeg != null)
                    {
                        if (model.SelectedTeg.FirstOrDefault() == "1")
                        {
                            if (m.whoLikes != null)
                            {
                                var whoLikes = m.whoLikes;
                                if (!m.whoLikes.Contains(currentPerson))
                                    m.whoLikes = whoLikes + currentPerson + " ";
                            }
                            else m.whoLikes = currentPerson + " ";
                            if (m.whoDislikes != null)
                                if (m.whoDislikes.Contains(currentPerson))
                                {
                                    var whoDislikes = m.whoDislikes;
                                    m.whoDislikes = whoDislikes.Replace(currentPerson, " ");
                                }
                        }
                        else if (model.SelectedTeg.FirstOrDefault() == "2")
                        {
                            if (m.whoDislikes != null)
                            {
                                var whoDislikes = m.whoDislikes;
                                if (!m.whoDislikes.Contains(currentPerson))
                                    m.whoDislikes = whoDislikes + currentPerson + " ";
                            }
                            else m.whoDislikes = currentPerson + " ";
                            if (m.whoLikes != null)
                                if (m.whoLikes.Contains(currentPerson))
                                {
                                    var whoLikes = m.whoLikes;
                                    m.whoLikes = whoLikes.Replace(currentPerson, " ");
                                }
                        }
                    }
                    if (m.whoLikes != null)
                        if (m.whoLikes.Contains(currentPerson))
                        {
                            model.youAreLikeThisArticle = true;
                            model.youAreDislikeThisArticle = false;
                        }
                        else model.youAreLikeThisArticle = false;
                    if (m.whoDislikes != null)
                        if (m.whoDislikes.Contains(currentPerson))
                        {
                            model.youAreDislikeThisArticle = true;
                            model.youAreLikeThisArticle = false;
                        }
                        else model.youAreDislikeThisArticle = false;
                    db.SaveChanges();
                }
            }
            //return View("ShowArticles", model);
        }
    }
}