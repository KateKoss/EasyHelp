using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    [Table("ChatMessages")]
    public class ChatMessageModel
    {
        [Key]        
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTimeSent { get; set; }
        public bool isMessageSent { get; set; }
    }
}