using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class ChatUsers
    {
        public int ID { get; set; }

        public virtual ApplicationUser user { get; set; }

        public virtual Chat chat { get; set; }
    }
}