﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication1.Models
{
    public class ProfileModel
    {
        public IEnumerable<string> articleName { get; set; }
        public IEnumerable<string> articleText { get; set; }
        public IEnumerable<SelectListItem> articleNames { get; set; }
        public string About_me { get; set; }
        public string Name { get; set; }
        public byte[] UserPhoto { get; set; }
        public IEnumerable<string> SelectedTeg { get; set; }
        public IEnumerable<SelectListItem> TegList { get; set; }
        public string MyTegs { get; set; }
        public string TextSearhAssoasiates { get; set; }
        public string TextSearhAssistants { get; set; }
        // public IEnumerable<SelectListItem> TegList { get; set; }
        [Key]
        public string UserName { get; set; }
        public bool flag_about_me { get; set; }
    }
    public class ProfileContext : DbContext
    {
        public ProfileContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ProfileModel> ProfileDb { get; set; }
    }
    [Table("Profile")]
    //public class ProfileModel
    //{
    //    public string articleName { get; set; }
    //    public string articleText { get; set; }
    //    public string About_me { get; set; }
    //    public string Name { get; set; }
    //    public byte[] UserPhoto { get; set; }
    //    public IEnumerable<string> SelectedTeg { get; set; }
    //    public IEnumerable<SelectListItem> TegList { get; set; }
    //    public string MyTegs { get; set; }
    //    public string TextSearhAssoasiates { get; set; }
    //    public string TextSearhAssistants { get; set; }
       
    //    [Key]
    //    public string UserName { get; set; }
    //}
}