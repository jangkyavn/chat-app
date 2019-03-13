using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Data.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }

        [ForeignKey("UserName")]
        public virtual User User { get; set; }
    }
}
