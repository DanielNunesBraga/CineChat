using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Chat
    {

        public int ID { get; set; }

        public string title { get; set; }

        public string password { get; set; }

        public virtual ApplicationUser admin { get; set; }

        public virtual Movie movie { get; set; }

        public virtual Category category { get; set; }

        public virtual ICollection<Post> posts { get; set; }

    }
}