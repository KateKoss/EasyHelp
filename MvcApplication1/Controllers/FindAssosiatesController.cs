using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Contexts;

namespace MvcApplication1.Controllers
{
    public class FindAssosiatesController : Controller
    {
        //
        // GET: /FindAssosiates/
        List<MentorsModel> usersList = new List<MentorsModel>();

        public ActionResult Index()
        {
            FindAssosiatesModel model = new FindAssosiatesModel();
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "картопля",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "курка",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "борщ",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "вареники",
                Value = "4"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "бутерброд",
                Value = "5"

            };
            listSelectListItems.Add(selectList);


            model.TegList1 = listSelectListItems;
            listSelectListItems = new List<SelectListItem>();

            selectList = new SelectListItem()
            {
                Text = "в лісі",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "в горах",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "на морі",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "вдома",
                Value = "4"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "не люблю",
                Value = "5"

            };
            listSelectListItems.Add(selectList);
            model.TegList2 = listSelectListItems;

            
            
            model.users = usersList;

            return View("FindAssosiates", model);
        }        

        [HttpPost]
        public ActionResult RetakeTest(ProfileModel model)
        {
            //пройти тест заново
            return View("FindAssosiates", model);
        }

        [HttpPost]
        public ActionResult ConfirmTest(FindAssosiatesModel model)
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                model.user = currentPerson;
                if (model.SelectedTeg1 != null)
                {
                    model.questionId = 1;
                    model.valueOfanswer = Convert.ToInt32(model.SelectedTeg1.Last());
                    model.id = model.user + '_' + model.questionId;
                    var id = model.id;
                    int count = db.FindAssosiatesModel.Count(x => x.user == currentPerson && x.questionId == model.questionId);
                    if (count == 0)
                    {
                        db.FindAssosiatesModel.Add(new FindAssosiatesModel { id = id });
                        db.SaveChanges();
                        var m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == id);
                        m.questionId = model.questionId;
                        m.user = model.user;
                        m.valueOfanswer = model.valueOfanswer;
                        db.SaveChanges();
                    }
                    else
                    {
                        var m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == model.id);
                        if (m != null)
                            m.valueOfanswer = model.valueOfanswer;
                        else
                        {
                            db.FindAssosiatesModel.Add(new FindAssosiatesModel { id = id });
                            db.SaveChanges();
                            m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == id);
                            m.questionId = model.questionId;
                            m.user = model.user;
                            m.valueOfanswer = model.valueOfanswer;
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                    }
                }
                if (model.SelectedTeg2 != null)
                {
                    model.questionId = 2;
                    model.valueOfanswer = Convert.ToInt32(model.SelectedTeg2.Last());
                    model.id = model.user + '_' + model.questionId;
                    var id = model.id;
                    int count = db.FindAssosiatesModel.Count(x => x.user == currentPerson && x.questionId == model.questionId);
                    if (count == 0)
                    {
                        
                        db.FindAssosiatesModel.Add(new FindAssosiatesModel { id = id});
                        db.SaveChanges();
                        var m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == id);
                        m.questionId = model.questionId;
                        m.user = model.user;
                        m.valueOfanswer = model.valueOfanswer;
                        db.SaveChanges();
                    }
                    else
                    {
                        var m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == id);
                        if (m != null)
                            m.valueOfanswer = model.valueOfanswer;
                        else
                        {
                            db.FindAssosiatesModel.Add(new FindAssosiatesModel { id = id });
                            db.SaveChanges();
                            m = db.FindAssosiatesModel.SingleOrDefault(x => x.id == model.id);
                            m.questionId = model.questionId;
                            m.user = model.user;
                            m.valueOfanswer = model.valueOfanswer;
                            
                        }
                        db.SaveChanges();
                    }
                }
                MentorsModel ment = new MentorsModel();
                Algorithm al = new Algorithm();
                List<DataPoint> l = new List<DataPoint>();
                DataPoint dp = new DataPoint(2);

                //var results = db.FindAssosiatesModel.GroupBy( p => p.user, p => p.questionId, p => p.answer, (key, q, a) => new { User = key, Questions = q.ToList(), Answers = a.toList() });
                var f = db.FindAssosiatesModel.GroupBy(x => x.user);
                foreach (var i in f)
                {
                    dp = new DataPoint(2);
                    var q = i.Where(x => x.questionId == 1).FirstOrDefault();
                    dp.a[0] = q.valueOfanswer;
                    dp.pointId = q.user;
                    
                    q = i.Where(x => x.questionId == 2).SingleOrDefault();
                    dp.a[1] = q.valueOfanswer;
                    l.Add(dp);
                }
                string[] ids = al.Init(currentPerson, l);
                using (CustomDbContext db2 = new CustomDbContext())
                {
                    foreach(var item in ids)
                    {
                        ment = new MentorsModel();
                        if (item != null && item !=currentPerson)
                        {
                            var w = db2.ProfileModel.SingleOrDefault(x => x.UserName == item);
                            ment.UserName = w.UserName;
                            ment.Name = w.Name;
                            ment.UserPhoto = w.UserPhoto;
                            usersList.Add(ment);
                        }
                    }
                }
                
                
            }
            //пройти тест заново
            //return RedirectToAction("Index");
            //FindAssosiatesModel model = new FindAssosiatesModel();
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            SelectListItem selectList = new SelectListItem()
            {
                Text = "картопля",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "курка",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "борщ",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "вареники",
                Value = "4"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "бутерброд",
                Value = "5"

            };
            listSelectListItems.Add(selectList);


            model.TegList1 = listSelectListItems;
            listSelectListItems = new List<SelectListItem>();

            selectList = new SelectListItem()
            {
                Text = "в лісі",
                Value = "1",
                Selected = true

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "в горах",
                Value = "2"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "на морі",
                Value = "3"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "вдома",
                Value = "4"

            };
            listSelectListItems.Add(selectList);
            selectList = new SelectListItem()
            {
                Text = "не люблю",
                Value = "5"

            };
            listSelectListItems.Add(selectList);
            model.TegList2 = listSelectListItems;



            model.users = usersList;

            return View("FindAssosiates", model);
        }

    }
}
