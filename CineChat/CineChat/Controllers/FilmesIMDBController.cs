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
using CineChat.Models;
using Microsoft.AspNet.Identity;
using CineChat.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;

namespace CineChat.Controllers
{
    public class FilmesIMDBController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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
                movies = apiSearch(searchString, false);
            }
            ViewBag.searchValue = searchString;
            return View(movies);
        }
        
        
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        public ActionResult Like(string imdbID)
        {
            IEnumerable<FilmesIMDB> imdbmovie = new List<FilmesIMDB>();
            string searchString = imdbID;
            if (!String.IsNullOrEmpty(searchString))
            {
                imdbmovie = apiSearch(searchString, true);
            }

            Movie newmovie = new Movie();
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            var imdb_movie = imdbmovie.FirstOrDefault();
            var checkmovie = db.movie.FirstOrDefault(m => m.ImdbID == imdb_movie.ImdbID);
            
            if (checkmovie == null)
            {
                newmovie.ImdbID = imdb_movie.ImdbID;
                newmovie.poster = imdb_movie.Poster;
                newmovie.title = imdb_movie.Title;
                newmovie.description = imdb_movie.Plot;
                newmovie.ratingImdb = imdb_movie.imdbRating;
                DateTime tempDate;
                if (DateTime.TryParse(imdb_movie.Released, out tempDate) == true)
                {
                    // succeeded ...
                    newmovie.releasedate = tempDate;
                }
                else
                {
                    newmovie.releasedate = DateTime.Now;
                }

                int temptime;
                string time = "";

                for (int i = 0; i < imdb_movie.RunTime.Length; i++)
                {
                    if (Char.IsDigit(imdb_movie.RunTime[i]))
                        time += imdb_movie.RunTime[i];
                }
                

                if (Int32.TryParse(time, out temptime))
                {
                    DateTime dt = DateTime.Now.Date;
                    dt = dt.AddMinutes(temptime);
                    newmovie.duration = dt;
                }
                else
                {
                    newmovie.duration = DateTime.Now;
                }

                db.movie.Add(newmovie);
                currentUser.likes.Add(newmovie);
                
            }
            else
            {
                currentUser.likes.Add(checkmovie);
            }
            db.SaveChanges();
            return RedirectToAction("MyMovies", "Movies");
        }

        private IEnumerable<FilmesIMDB> apiSearch(string searchString, bool like)
        {
            HttpClient myClient = new HttpClient();
            // New code:
            myClient.BaseAddress = new Uri("http://www.omdbapi.com/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<FilmesIMDB> result = new List<FilmesIMDB>();
            if (!like)
            {
                //alterei o codigo de maneira a responder da mesma maneira como se estivesse a trabalhar como async
                HttpResponseMessage response = myClient.GetAsync("?s=" + searchString + "&r=json&type=movie").Result;
                //HttpResponseMessage response = myClient.GetAsync("?s=" + searchString + "&y=&plot=short&r=json&type=movie").Result;
                if (response.IsSuccessStatusCode)
                {
                    //link: http://stackoverflow.com/questions/19448690/how-to-consume-a-webapi-from-asp-net-web-api-to-store-result-in-database
                    var movies = response.Content.ReadAsAsync<SearchImdb>().Result;
                    if (movies != null && movies.Search != null)
                    {
                        foreach (var movie in movies.Search)
                        {
                            result.Add(IMDB_Single_Search(movie.ImdbID, myClient));
                        }
                    }
                }
            }
            else
            {
                result.Add(IMDB_Single_Search(searchString, myClient));
            }
            return result;
        }

        private FilmesIMDB IMDB_Single_Search(string ImdbID, HttpClient myClient)
        {
            HttpResponseMessage response_detail = myClient.GetAsync("?i=" + ImdbID + "&plot=short&r=json").Result;
            if (response_detail.IsSuccessStatusCode)
            {
                var movies_detail = response_detail.Content.ReadAsAsync<FilmesIMDB>().Result;
                return movies_detail;
            }
            else
                return new FilmesIMDB();
        }


        public class SearchImdb
        {
            public List<FilmesIMDB> Search { get; set; }
        }
       
    }
}
