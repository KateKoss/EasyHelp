using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class MentorsModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public byte[] UserPhoto { get; set; }
        public string Tegs { get; set; }
        public string AboutMe { get; set; }

            //public Mentor(string UserName, string Name, byte[] UserPhoto)
            //{
            //    this.UserName = UserName;
            //    this.Name = Name;
            //    this.UserPhoto = UserPhoto;                
            //}                                
       

        //public List<Mentor> mentors = new List<Mentor>();
    }
}