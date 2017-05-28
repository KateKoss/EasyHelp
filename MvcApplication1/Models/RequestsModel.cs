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
    [Table("Request")]
    public class RequestModel
    {         

        public RequestModel(string requestId, string requestName, string requestText, IEnumerable<SelectListItem> TegList, string requestState)
        {
            this.requestId = requestId;
            this.requestName = requestName;
            this.requestText = requestText;
            this.TegList = TegList;
            this.requestState = requestState;
        }
        public RequestModel() { }

        [Key]
        public string requestId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введіть назву заявки.")]
        public string requestName { get; set; }
        public string requestText { get; set; }
        public IEnumerable<string> SelectedTeg { get; set; }
        public IEnumerable<SelectListItem> TegList { get; set; }
        public string requestState { get; set; } //Есть 3 состояния "request resolved", "request not resolved", "request canceled"
        public string createdBy { get; set; }
            
    }
    public class RequestsList
    { 
        public List<RequestModel> reqests = new List<RequestModel>();        
    }
}