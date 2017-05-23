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
        public ActionResult LoadStudentRequests(RequestsModel m, string user)
        {
            //в модель передать все активные заявки из бд 
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_1", "Help with Java", "bla-bla1", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            req = new RequestsModel.Request("user1_2", "Help with C#", "bla-bla2", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);

            return View("StudentRequests", requestsModel.reqests);
        }

        [HttpGet]
        public ActionResult ActiveRequests()
        {
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_1", "Help with 1...", "bla-bla1", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);  //если заявка не решена и не отменена отображаем как активные
            req = new RequestsModel.Request("user1_2", "Help with 2...", "bla-bla2", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);

            return View("StudentRequests", requestsModel.reqests);
        }

        [HttpGet]
        public ActionResult ResolvedRequests()
        {
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_3", "Help with 1...", "bla-bla1", null, "request resolved");
            if (req.requestState != "request not resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            if (Request.IsAjaxRequest() == true)
            {
                req = new RequestsModel.Request("user1_4", "Help with 2...", "bla-bla2", null, "request resolved");
                if (req.requestState != "request not resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            }
            req = new RequestsModel.Request("user1_4", "Help with 2...", "bla-bla2", null, "request resolved");
            if (req.requestState != "request not resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            return View("StudentRequests", requestsModel.reqests);
        }
        [HttpGet]
        public ActionResult CanceledRequests()
        {
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_5", "Help with 1...", "bla-bla1", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);
            req = new RequestsModel.Request("user1_6", "Help with 2...", "bla-bla2", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);
            return View("StudentRequests", requestsModel.reqests);
        }
        [HttpGet]
        public ActionResult MarkAsResolved(RequestsModel resolvThisReq, string requestId)
        {
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_5", "Help with 1...", "bla-bla1", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);  //если заявка не решена и не отменена отображаем как активные
            req = new RequestsModel.Request("user1_6", "Help with 2...", "bla-bla2", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);
            if (requestsModel.reqests.Contains(resolvThisReq.reqests[0]))
            {
                requestsModel.reqests.Remove(resolvThisReq.reqests[0]);
            }

            return View("Index", requestsModel.reqests);
        }

        [HttpGet]
        public ActionResult MarkAsCanseled(RequestsModel resolvThisReq, string requestId)
        {
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("user1_5", "Help with 1...", "bla-bla1", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);  //если заявка не решена и не отменена отображаем как активные
            req = new RequestsModel.Request("user1_6", "Help with 2...", "bla-bla2", null, "request canceled");
            if (req.requestState != "request not resolved" && req.requestState != "request resolved") requestsModel.reqests.Add(req);
            return View("Index", requestsModel.reqests);
        }

        //--------------------------Создание/сохранение заявки-----------------------------------------------

        [HttpGet]
        public ActionResult CreateRequest()
        {

            return View("CreateRequest");
        }


        public ActionResult SaveReqest(RequestsModel.Request req)
        {
            req.requestState = "request not resolved";
            //сохранить заявку в бд
            return View("CreateRequest");
        }

    }
}