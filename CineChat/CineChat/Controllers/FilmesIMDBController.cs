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
            
            IEnumerable<FilmesIMDB> filmes = new List<FilmesIMDB>();
            string searchString = id;
            if (!String.IsNullOrEmpty(searchString))
            {
                filmes = apiSearch(searchString);
            }
            return View(filmes);
        }

        private IEnumerable<FilmesIMDB> apiSearch(string searchString)
        {
            HttpClient myClient = new HttpClient();
            // New code:
            myClient.BaseAddress = new Uri("http://www.omdbapi.com/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //alterei o codigo de maneira a responder da mesma maneira como se estivesse a trabalhar como async
            HttpResponseMessage response = myClient.GetAsync("?t=" + searchString + "&type=movie&y=&plot=short&r=json").Result;

            if (response.IsSuccessStatusCode)
            {
                //link: http://stackoverflow.com/questions/19448690/how-to-consume-a-webapi-from-asp-net-web-api-to-store-result-in-database
                var filmes = response.Content.ReadAsAsync<FilmesIMDB>().Result;
                List<FilmesIMDB> result = new List<FilmesIMDB>();
                result.Add(filmes);
                return result;
            }
            return new List<FilmesIMDB>();
        }
       
    }
}
