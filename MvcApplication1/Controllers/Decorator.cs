using MvcApplication1.Contexts;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers
{
    public class Decorator
    {
        public static string id { get; set; }
        public static Singleton s;
        public abstract class SavaArticle
        {
            public abstract void saveToDb(ArticleModel model);
        }

        public class ImplementSave: SavaArticle {
            
            public ImplementSave() { }

            public override void saveToDb(ArticleModel model)
            {
                s = Singleton.Instance;
                if (model.articleTitle != null)
                    using (CustomDbContext db = new CustomDbContext())
                    {
                        string articleid;
                        string currentPerson = s.user;

                        //чи створював користувач вже статті
                        var myModel = db.ArticleModel.FirstOrDefault(x => x.createdBy == s.user);
                        if (myModel == null)
                        {
                            articleid = currentPerson + '_' + 1;
                            //якщо ні, то id статті 1, бо ця стаття в нього перша
                            db.ArticleModel.Add(new ArticleModel { articleID = articleid, createdBy = currentPerson, articleTitle = model.articleTitle });
                            if (myModel != null)//якщо є
                            {
                                myModel.articleTitle = model.articleTitle;
                                myModel.articleText = model.articleText;
                                //myModel.isPublished = false;
                            }
                            db.SaveChanges();
                            Decorator.id = articleid;
                        }
                        else
                            //якщо користувач обрав існуючу статтю для редагування
                            if (model.articleID != null)
                            {
                                //articleid = currentPerson + "_" + model.articleID;
                                //перевіряємо чи власне є ця стаття у бд
                                myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == model.articleID);
                                if (myModel != null)//якщо є
                                {
                                    myModel.articleTitle = model.articleTitle;
                                    myModel.articleText = model.articleText;
                                    //myModel.isPublished = false;
                                    db.SaveChanges();
                                }
                                else
                                {

                                    db.ArticleModel.Add(new ArticleModel { articleID = model.articleID, createdBy = currentPerson, articleTitle = model.articleTitle });
                                    myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == model.articleID);
                                    if (myModel != null)
                                    {
                                        myModel.articleTitle = model.articleTitle;
                                        myModel.articleText = model.articleText;
                                        //myModel.isPublished = false;
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
                                    //myModel.isPublished = false;
                                }
                                db.SaveChanges();
                                Decorator.id = articleid;
                            }


                    }
            }
        }

        /**
        Абстрактний клас для декораторів
        */
        public abstract class DecoratedSave : SavaArticle
        {
            protected SavaArticle decoratedSaveArticle;

            public abstract override void saveToDb(ArticleModel model);
        }

        /**
        Декоратор, який зберігає статтю як чернетку
        */
        public class SaveToDraft : DecoratedSave
        {

            public SaveToDraft(SavaArticle sa)
            {
                decoratedSaveArticle = sa;
            }

            public SaveToDraft() { }

            public override void saveToDb(ArticleModel model)
            {
                s = Singleton.Instance;
                if (model.articleTitle != null)
                    using (CustomDbContext db = new CustomDbContext())
                    {
                        var myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == model.articleID);
                            if (myModel != null)
                            {
                                myModel.articleTitle = model.articleTitle;
                                myModel.articleText = model.articleText;
                                myModel.isPublished = false;
                            }
                            db.SaveChanges();

                            model.isPublished = false;

                    }
            }
        }

        /**
        Декоратор, який публікує статтю
        */
        public class Publish : DecoratedSave
        {

            public Publish(SavaArticle sa)
            {
                decoratedSaveArticle = sa;
            }
            public Publish() { }
            public override void saveToDb(ArticleModel model)
            {
                s = Singleton.Instance;
                if (model.articleTitle != null)
                    using (CustomDbContext db = new CustomDbContext())
                    {
                        var myModel = db.ArticleModel.FirstOrDefault(x => x.articleID == model.articleID);
                        if (myModel != null)
                        {
                            myModel.articleTitle = model.articleTitle;
                            myModel.articleText = model.articleText;
                            myModel.isPublished = true;
                        }
                        db.SaveChanges();

                        model.isPublished = true;

                    }            
            }
        }

    }
}