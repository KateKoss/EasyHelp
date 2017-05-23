using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    [Table("Article")]
    public class ArticleModel
    {        
        [Key]
        public string articleID { get; set; }
        public string createdBy { get; set; }
        public IEnumerable<string> articleName { get; set; }
        public IEnumerable<SelectListItem> articleNames { get; set; }
        [Required(AllowEmptyStrings=false,ErrorMessage = "Введіть назву саттті.")]
        public string articleTitle { get; set; }
        public string articleText { get; set; }
        public bool isPublished { get; set; }
        
    }
}