using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Chat
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string title { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        public virtual ApplicationUser admin { get; set; }

        public virtual Movie movie { get; set; }

        public virtual Category category { get; set; }

        public virtual ICollection<Post> posts { get; set; }

        public virtual ICollection<ChatUsers> logeduser { get; set; }

    }
}