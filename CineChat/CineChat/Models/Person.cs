using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Person
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string name { get; set; }

        public virtual ICollection<Characters> characters { get; set; }

        /*public virtual Writer writer { get; set; }

        public virtual Director director { get; set; }*/
    }
}