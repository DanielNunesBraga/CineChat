using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CineChat.Models
{
    public class FilmesIMDB
    {
        public int Id { get; set; }
        public string Title { get; set; }
//        public string Category { get; set; }
        public int Year { get; set; }
        [DataType(DataType.Date)]
        public DateTime Released { get; set; }
        public string RunTime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public int Metascore { get; set; }
        public float RatingIMDB { get; set; }
        public string ImdbID { get; set; }
        public string IMDBVotes { get; set; }
        public string Type { get; set; }
        public bool Response { get; set; }





    }
}