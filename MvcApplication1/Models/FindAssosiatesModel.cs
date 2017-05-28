using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.Models
{
    [Table("FindAssosiates")]
    public class FindAssosiatesModel
    {
        public IEnumerable<string> SelectedTeg1 { get; set; }
        public IEnumerable<SelectListItem> TegList1 { get; set; }
        public IEnumerable<string> SelectedTeg2 { get; set; }
        public IEnumerable<SelectListItem> TegList2 { get; set; }

        [Key]
        public string id { get; set; }
        public string user { get; set; }
        public int questionId { get; set; }
        public int valueOfanswer { get; set; }

        [NotMapped]
        public List<MvcApplication1.Models.MentorsModel> users = new List<MvcApplication1.Models.MentorsModel>();

    }
}