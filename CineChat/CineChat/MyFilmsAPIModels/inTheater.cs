using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.MyFilmsAPIModels
{
    public class inTheater
    {
        public List<Movie> movies { get; set; }

        public string openingThisWeek { get; set; }
        public string inTheatersNow { get; set; }
        public string date { get; set; }
        
    }
}