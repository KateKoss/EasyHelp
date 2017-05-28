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
    public class RequestsController : Controller
    {
        [HttpGet]
        public ActionResult LoadStudentRequests()
        {
            RequestsList requestsModel = new RequestsList();
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            using (CustomDbContext db = new CustomDbContext())
            {
                var r = db.RequestsModel.Where(x => x.createdBy == currentPerson && x.requestState == "request not resolved");
                if (r!=null)
                    foreach (var item in r)
                    {
                        requestsModel.reqests.Add(item);
                    }
            }
                //в модель передать все активные заявки из бд 
                
                //RequestModel req = new RequestModel("user1_1", "Help with Java", "bla-bla1", null, "request not resolved");
                //if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
                //req = new RequestModel("user1_2", "Help with C#", "bla-bla2", null, "request not resolved");
                //if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            
            return View("StudentRequests", requestsModel.reqests);
        }        

        [HttpGet]
        public ActionResult ResolvedRequests()
        {
            RequestsList requestsModel = new RequestsList();
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            using (CustomDbContext db = new CustomDbContext())
            {
                var r = db.RequestsModel.Where(x => x.createdBy == currentPerson && x.requestState == "request resolved");
                if (r!=null)
                    foreach (var item in r)
                    {
                        requestsModel.reqests.Add(item);
                    }
            }
            
            RequestModel req = new RequestModel("user1_3", "Help with 1...", "bla-bla1", null, "request resolved");
            if (req.requestState != "request not resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            
            
            return View("StudentRequests", requestsModel.reqests);
        }
        [HttpGet]
        public ActionResult CanceledRequests()
        {
            RequestsList requestsModel = new RequestsList();
            string currentPerson;
            if (Request.Cookies["UserId"] != null)
                currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            else currentPerson = "user1";
            using (CustomDbContext db = new CustomDbContext())
            {
                var r = db.RequestsModel.Where(x => x.createdBy == currentPerson && x.requestState == "request canceled");
                if (r!=null)
                    foreach (var item in r)
                    {
                        requestsModel.reqests.Add(item);
                    }
            }
            return View("StudentRequests", requestsModel.reqests);
        }
        [HttpGet]
        public ActionResult MarkAsResolved(RequestsList resolvThisReq, string requestId)
        {
            //RequestsList requestsModel = new RequestsList();
            
            using (CustomDbContext db = new CustomDbContext())
            {
                var r = db.RequestsModel.SingleOrDefault(x => x.requestId == requestId);

                if (r != null)
                    r.requestState = "request resolved";
                db.SaveChanges();
            }
            //RequestModel req = new RequestModel("user1_5", "Help with 1...", "bla-bla1", null, "request canceled");
            //if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);  //если заявка не решена и не отменена отображаем как активные
            //req = new RequestModel("user1_6", "Help with 2...", "bla-bla2", null, "request canceled");
            //if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);

            return RedirectToAction("ResolvedRequests");
        }

        [HttpGet]
        public ActionResult MarkAsCanceled(RequestsList resolvThisReq, string requestId)
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                var r = db.RequestsModel.SingleOrDefault(x => x.requestId == requestId);

                if (r != null)
                    r.requestState = "request canceled";
                db.SaveChanges();
            }
            //RequestsList requestsModel = new RequestsList();
            //RequestModel req = new RequestModel("user1_5", "Help with 1...", "bla-bla1", null, "request canceled");
            //if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);  //если заявка не решена и не отменена отображаем как активные
            //req = new RequestModel("user1_6", "Help with 2...", "bla-bla2", null, "request canceled");
            //if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);
            return RedirectToAction("CanceledRequests");
        }

        //--------------------------Создание/сохранение заявки-----------------------------------------------

        [HttpGet]
        public ActionResult CreateRequest()
        {

            return View("CreateRequest");
        }


        public ActionResult SaveReqest(RequestModel req)
        {
            using (CustomDbContext db = new CustomDbContext())
            {
                string currentPerson;
                if (Request.Cookies["UserId"] != null)
                    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
                else currentPerson = "user1";
                int count = db.RequestsModel.Count(x => x.createdBy == currentPerson);
                if (count == 0)
                {
                    req.requestId = currentPerson + "_1";                    
                }
                else
                {
                    req.requestId = currentPerson + "_" + (count + 1);
                }
                req.requestState = "request not resolved";
                req.createdBy = currentPerson;
                req.createdAt = DateTime.Now;
                if (req.requestName == null)
                    req.requestName = req.requestText.Substring(0, 10);
                db.RequestsModel.Add(
                //    new RequestModel { 
                //    requestId = req.requestId,
                //    requestName = req.requestName,
                //    requestText = req.requestText,
                //    requestState = req.requestState,
                //    createdBy = req.createdBy
                //}
                req
                );
                db.SaveChanges();                
                
            }

            return RedirectToAction("LoadStudentRequests");
        }

    }
}