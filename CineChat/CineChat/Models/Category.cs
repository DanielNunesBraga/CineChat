using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Category
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(60,MinimumLength = 3)]
        public string description { get; set; }

        public ICollection<Movie> movies { get; set; }

        public ICollection<Chat> chats { get; set; }
    }
}