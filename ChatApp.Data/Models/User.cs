using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Data.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
        public bool Connected { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
