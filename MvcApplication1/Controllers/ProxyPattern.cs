using MvcApplication1.Contexts;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ProxyPattern
    {
        // "Subject"

        public interface IGetArticle
        {
            ArticleModel getArticle(string currentPerson, bool onlyPublsihed);
        }

        // "RealSubject"

        class GetArticle : IGetArticle
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
                    //articleTitle = listSelectListItems
                };

                return myModel;
            }
        }

        // "Proxy Object"

        public class Proxy : IGetArticle
        {
            GetArticle get;
            public Proxy()
            {
                get = new GetArticle();
            }

            public ArticleModel getArticle(string currentPerson, bool onlyPublsihed)
            {
                return get.getArticle(currentPerson, onlyPublsihed);
            }
        }
    }
}