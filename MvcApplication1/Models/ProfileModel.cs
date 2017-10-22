using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication1.Models
{   
    [Table("Profile")]
    public class ProfileModel
    {
        //список менторів
        [NotMapped]
        public List<MvcApplication1.Models.ProfileModel> mentors = new List<MvcApplication1.Models.ProfileModel>();
        [NotMapped]
        public List<String> tegs = new List<String>();
        [NotMapped]
        public string searchMentor { get; set; }
        [NotMapped]
        public bool isMentor { get; set; }
        public string About_me { get; set; }
        public string Name { get; set; }
        public byte[] UserPhoto { get; set; }
        
        public IEnumerable<string> SelectedTeg { get; set; }
        public IEnumerable<SelectListItem> TegList { get; set; }
        public string MyTegs { get; set; }
        public int Rate { get; set; }
        
        [Key]
        public string UserName { get; set; }        
    } 
}