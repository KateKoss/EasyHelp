using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;

namespace MvcApplication1.Models
{
    public class RequestsModel
    {
        public class Request
        {
            public Request(string requestName, string requestText, IEnumerable<SelectListItem> TegList, string requestState)
            {
                this.requestName = requestName;
                this.requestText = requestText;
                this.TegList = TegList;
                this.requestState = requestState;
            }
            public string requestName { get; set; }
            public string requestText { get; set; }
            public IEnumerable<string> SelectedTeg { get; set; }
            public IEnumerable<SelectListItem> TegList { get; set; }
            public string requestState { get; set; } //Есть 3 состояния "request resolved", "request not resolved", "request canceled"
        }

        public List<Request> reqests = new List<Request>();
    }
}