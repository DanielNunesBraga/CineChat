using CineChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;

namespace CineChat.Controllers
{
    public class FilmesIMDBController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public ActionResult Search(string id)
        {
            
            IEnumerable<FilmesIMDB> movies = new List<FilmesIMDB>();
            string searchString = id;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = apiSearch(searchString);
            }
            return View(movies);
        }

        private IEnumerable<FilmesIMDB> apiSearch(string searchString)
        {
            HttpClient myClient = new HttpClient();
            // New code:
            myClient.BaseAddress = new Uri("http://www.omdbapi.com/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //alterei o codigo de maneira a responder da mesma maneira como se estivesse a trabalhar como async
            HttpResponseMessage response = myClient.GetAsync("?s=" + searchString + "&r=json&type=movie").Result;
            //HttpResponseMessage response = myClient.GetAsync("?s=" + searchString + "&y=&plot=short&r=json&type=movie").Result;

            if (response.IsSuccessStatusCode)
            {
                List<FilmesIMDB> result = new List<FilmesIMDB>();
                //link: http://stackoverflow.com/questions/19448690/how-to-consume-a-webapi-from-asp-net-web-api-to-store-result-in-database
                var movies = response.Content.ReadAsAsync<SearchImdb>().Result;
                if (movies != null)
                {
                    foreach (var movie in movies.Search)
                    {
                        HttpResponseMessage response_detail = myClient.GetAsync("?i=" + movie.ImdbID + "&plot=short&r=json").Result;
                        if (response_detail.IsSuccessStatusCode)
                        {
                            var movies_detail = response_detail.Content.ReadAsAsync<FilmesIMDB>().Result;
                            if (movies_detail != null)
                            {
                                result.Add(movies_detail);
                            }
                        }
                    }
                    return result;
                }
            }
            return new List<FilmesIMDB>();
        }

        public class SearchImdb
        {
            public List<FilmesIMDB> Search { get; set; }
        }
       
    }
}
