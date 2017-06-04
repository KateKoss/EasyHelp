using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers
{
    public class Memento
    {
        public readonly string UserName;
        public readonly string Name;
        public readonly byte[] UserPhoto;
        public readonly string Tegs;
        public readonly string AboutMe;

        public Memento(string UserName, string Name, byte[] UserPhoto, string Tegs, string AboutMe)
        {
            this.UserName = UserName;
            this.Name = Name;
            this.UserPhoto = UserPhoto;
            this.Tegs = Tegs;
            this.AboutMe = AboutMe;
        }
    }
}