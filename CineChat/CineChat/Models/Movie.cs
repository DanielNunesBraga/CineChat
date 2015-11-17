using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class Movie
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string ImdbID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string title { get; set; }

        [DataType(DataType.Date)]
        public DateTime releasedate { get; set; }

        [DataType(DataType.Time)]
        public DateTime duration { get; set; }

        public double ratingImdb { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string description { get; set; }

        public string poster { get; set; }

        public virtual ICollection<Category> categories{ get; set; }

        public virtual ICollection<Characters> characters { get; set; }

        public virtual ICollection<Director> directors { get; set; }

        public virtual ICollection<Writer> writers { get; set; }

        public virtual ICollection<ApplicationUser> likes { get; set; }

        public virtual ICollection<Rates> rates { get; set; }




    }
}