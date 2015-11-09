using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Writer
    {
        public int ID { get; set; }

        public virtual Person person { get; set; }

        public virtual ICollection<Movie> movies { get; set; }

    }
}