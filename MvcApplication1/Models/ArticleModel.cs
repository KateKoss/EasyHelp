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
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string articleID { get; set; }
        public string createdBy { get; set; }

        //[Required(AllowEmptyStrings=false, ErrorMessage = "Введіть назву статті.")]
        public string articleTitle { get; set; }
        public string articleText { get; set; }
        public bool isPublished { get; set; }
        public DateTime dateOfLastEditing { get; set; }
        public DateTime dateOfCreation { get; set; }
        public string tagList { get; set; }
        public string whoLikes { get; set; }
        public string whoDislikes { get; set; }
        public List<string> chosenTags { get; set; }
        public IEnumerable<string> SelectedTeg { get; set; }
        public IEnumerable<SelectListItem> TegList { get; set; }
        //нужна дата создания
        [NotMapped]
        public bool youAreLikeThisArticle { get; set; }
        [NotMapped]
        public bool youAreDislikeThisArticle { get; set; }

    }
}