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
    public class MyFilmsApiComingSoonController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyFilmsAPI

        public ActionResult Search()
        {
            IEnumerable<inTheater> inTheaters = apiSearch();
            if (inTheaters == null)
                return View("Index");
            else
            {
                List<Movie> movies = new List<Movie>();
                foreach(var inTheather in inTheaters){
                    movies = movies.Concat(inTheather.movies).ToList();
                }

                //return View(movies.ToList());
                return View(inTheaters.ToList());
            }
        }

        public static IEnumerable<inTheater> apiSearch()
        {
            HttpClient myClient = new HttpClient();
            myClient.BaseAddress = new Uri("http://api.myapifilms.com/imdb/comingSoon/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string token = "a3a9d437-81bc-425f-a1ca-3583f0dbe27f";

            List<inTheater> result = new List<inTheater>();

            HttpResponseMessage response = myClient.GetAsync("?token=" + token + "&format=json&language=en-us&date="+DateTime.Now.Year).Result;

            
            if (response.IsSuccessStatusCode)
            {

                MySearch comingSoon = new MySearch();
                comingSoon = response.Content.ReadAsAsync<MySearch>().Result;
                result = comingSoon.data.inTheaters;
            }
            return result;
        }
        
        private class MySearch
        {
            
            public About about { get; set; }
            public Data data { get; set; }
        }
    }


}


