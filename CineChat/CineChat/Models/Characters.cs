using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Characters
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string name { get; set; }

        [Required]
        public virtual Person actor { get; set; }

        public virtual Movie movie { get; set; }

    }
}