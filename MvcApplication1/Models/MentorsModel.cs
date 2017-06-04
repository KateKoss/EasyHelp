using MvcApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class MentorsModel
    {
        public Memento mem;

        public string UserName { get; set; }
        public string Name { get; set; }
        public byte[] UserPhoto { get; set; }
        public string Tegs { get; set; }
        public string AboutMe { get; set; }

        public MentorsModel ()
        {
            this.mem = new Memento(UserName, Name, UserPhoto, Tegs, AboutMe);
        }

            //public Mentor(string UserName, string Name, byte[] UserPhoto)
            //{
            //    this.UserName = UserName;
            //    this.Name = Name;
            //    this.UserPhoto = UserPhoto;                
            //}                                
       

        //public List<Mentor> mentors = new List<Mentor>();
    }
}