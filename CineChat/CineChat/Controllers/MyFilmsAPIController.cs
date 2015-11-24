using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CineChat.DAL;
using CineChat.MyFilmsAPIModels;
using Microsoft.AspNet.Identity;
using CineChat.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;

namespace CineChat.Controllers
{
    public class MyFilmsAPIController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: MyFilmsAPI

        public ActionResult Search(string id)
        {
            IEnumerable<Movie> mymovie = apiSearch(id);
            if (mymovie != null)
                return View(mymovie.ToList());
            else
                return View("Index");
        }


        /*
        [System.Web.Mvc.HttpPost]
        public ActionResult Search()
        {
            return View();
        }*/
        private IEnumerable<Movie> apiSearch(string searchString)
        {
            HttpClient myClient = new HttpClient();
            // New code:

            myClient.BaseAddress = new Uri("http://api.myapifilms.com/imdb/idIMDB/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string token = "a3a9d437-81bc-425f-a1ca-3583f0dbe27f";

            List<Movie> result = new List<Movie>();
           
                //alterei o codigo de maneira a responder da mesma maneira como se estivesse a trabalhar como async
            HttpResponseMessage response = myClient.GetAsync("?title=" + searchString + "&token=" + token + "&format=json&language=en-us&aka=0&business=0&seasons=0&seasonYear=0&technical=0&filter=3&exactFilter=0&limit=10&forceYear=0&trailers=0&movieTrivia=0&awards=0&moviePhotos=0&movieVideos=0&actors=0&biography=0&uniqueName=0&filmography=0&bornAndDead=0&starSign=0&actorActress=0&actorTrivia=0&similarMovies=0&adultSearch=0").Result;
                //HttpResponseMessage response = myClient.GetAsync("?s=" + searchString + "&y=&plot=short&r=json&type=movie").Result;
                if (response.IsSuccessStatusCode)
                {

                    MySearch moviedata = new MySearch();
                    moviedata = response.Content.ReadAsAsync<MySearch>().Result;
                    result = moviedata.data.movies;
                }
                return result;
            }

        private class MySearch
        {
            public Data data { get; set; }
            public About about { get; set; }
       }
    }

}
