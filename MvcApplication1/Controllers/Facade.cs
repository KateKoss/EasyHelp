using MvcApplication1.Contexts;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class FacadePattern
    {
        public static string str { get; set; }
        public class Facade
        {
            Subsystem1 first;
            Subsystem2 second;
            ArticleModel model;
            string currentPerson;
            bool onlyPublished;

            public Facade(ArticleModel model, string currentPerson, bool onlyPublished)
            {
                this.first = new Subsystem1();
                this.second = new Subsystem2();
                this.currentPerson = currentPerson;
                this.model = model;
                this.onlyPublished = onlyPublished;
            }

            //Виклик методів з підсистеми 1
            public void workWithSelectedTeg()
            {
                first.Method(model, currentPerson, onlyPublished);
            }

            //Виклик методів з підсистеми 2
            public void workWithDb()
            {
                second.Method(model, currentPerson);
            }
        }



        // Клас підсистеми
        class Subsystem1
        {
            public ArticleModel getArticle(string currentPerson, bool onlyPublsihed)
            {
                List<SelectListItem> listSelectListItems = new List<SelectListItem>();
                using (CustomDbContext db = new CustomDbContext())
                {
                    if (!onlyPublsihed)
                        foreach (var article in db.ArticleModel.Where(x => x.createdBy == currentPerson))
                        {
                            SelectListItem selectList = new SelectListItem()
                            {
                                Text = article.articleTitle,
                                Value = article.articleID
                            };
                            listSelectListItems.Add(selectList);
                        }
                    else
                        foreach (var article in db.ArticleModel.Where(x => x.createdBy == currentPerson && x.isPublished == true))
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

                return myModel;
            }
            public void Method(ArticleModel model, string currentPerson, bool onlyPublished)
            {
                model.articleNames = getArticle(currentPerson, onlyPublished).articleNames;
                //var str = "";
                if (model.articleName != null)
                    foreach (string s in model.articleName)
                    {
                        str = model.articleNames.FirstOrDefault(x => x.Value == s).Text;
                        model.articleID = s;
                    }
            }
        }

        // Клас підсистеми
        class Subsystem2
        {
            public void Method(ArticleModel model, string currentPerson)
            {
                using (CustomDbContext db = new CustomDbContext())
                {
                    //string str = "";
                    var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == currentPerson);
                    if (myModel != null)
                    {
                        model.articleTitle = str;
                        if (FacadePattern.str != "")
                            model.articleText = myModel.articleText;
                    }

                }
            }
        }
    }
}