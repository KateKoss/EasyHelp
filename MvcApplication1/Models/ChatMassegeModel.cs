using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    [Table("ChatMasseges")]
    public class ChatMassegeModel
    {
        [Key]        
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Massege { get; set; }
        public DateTime DateTimeSent { get; set; }
    }
}