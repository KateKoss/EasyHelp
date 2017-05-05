using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcApplication1.Models
{
    

    public class ProfileModel
    {
        
        public string About_me { get; set; }
        [Key]
        public string UserName { get; set; }
        public bool flag_about_me { get; set; }
    }
    
}