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
            RequestsModel requestsModel = new RequestsModel();
            RequestsModel.Request req = new RequestsModel.Request("Help with Java", "bla-bla1", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            req = new RequestsModel.Request("Help with C#", "bla-bla2", null, "request not resolved");
            if (req.requestState != "request resolved" && req.requestState != "request canceled") requestsModel.reqests.Add(req);
            return View("StudentRequests", requestsModel.reqests);

        }
        public ActionResult ActiveRequests()
        {
            return View();
        }
        public ActionResult ResolvedRequests()
        {
            return View();
        }
        public ActionResult CanceledRequests()
        {
            return View();
        }
    }
}