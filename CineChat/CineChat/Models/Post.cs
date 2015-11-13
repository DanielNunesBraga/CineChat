using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Post
    {
        public int ID { get; set; }

        public string message { get; set; }

        public DateTime time { get; set; }

        public virtual ApplicationUser user { get; set; }

        public virtual Chat chat { get; set; }
    }
}