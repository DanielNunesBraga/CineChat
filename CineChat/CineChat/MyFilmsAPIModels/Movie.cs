using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineChat.MyFilmsAPIModels
{
    public class Movie
    {
        
        public string title { get; set; }
        public string originalTitle { get; set; }
        public string year { get; set; }
        public string releaseDate { get; set; }
        public string urlPoster { get; set; }
        public string plot { get; set; }
        public string simplePlot { get; set; }
        public string idIMDB { get; set; }
        public string urlIMDB { get; set; }
        public string rating { get; set; }
        public string metascore { get; set; }
        public string rated { get; set; }
        public string votes { get; set; }
        public string type { get; set; }

        public List<string> runtime { get; set; }
        public List<string> countries { get; set; }
        public List<string> languages { get; set; }
        public List<string> genres { get; set; }
        public List<string> filmingLocations { get; set; }

        public List<Director> directors { get; set; }
        public List<Writer> writers { get; set; }


        
    }
}