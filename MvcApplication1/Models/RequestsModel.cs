using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using System.Reflection;

namespace MvcApplication1.Models
{
    public class RequestsModel
    {
        public class Request
        {
            public Request(string requestId, string requestName, string requestText, IEnumerable<SelectListItem> TegList, string requestState)
            {
                this.requestName = requestName;
                this.requestText = requestText;
                this.TegList = TegList;
                this.requestState = requestState;
            }
            public string requestId { get; set; }
            public string requestName { get; set; }
            public string requestText { get; set; }
            public IEnumerable<string> SelectedTeg { get; set; }
            public IEnumerable<SelectListItem> TegList { get; set; }
            public string requestState { get; set; } //Есть 3 состояния "request resolved", "request not resolved", "request canceled"
        }

        public List<Request> reqests = new List<Request>();
    }

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
}