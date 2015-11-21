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
            return RedirectToAction("Index", "Home");
        }

        [System.Web.Mvc.Authorize]
        //[System.Web.Mvc.HttpPost]
        public ActionResult Search(string id)
        {
            //search list of movies
            IEnumerable<FilmesIMDB> movies = new List<FilmesIMDB>();
            string searchString = id;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = apiSearch(searchString, false);
            }
            ViewBag.successlike = TempData["successlike"];
            ViewBag.searchValue = searchString;
            return View(movies);
        }


        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult Like(string imdbID, string searchValue)
        {
            //add movie to mymovies list
            IEnumerable<FilmesIMDB> imdbmovie = new List<FilmesIMDB>();
            string searchString = imdbID;
            if (!String.IsNullOrEmpty(searchString))
            {
                imdbmovie = apiSearch(searchString, true);


                Movie newmovie = new Movie();
                string currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                var imdb_movie = imdbmovie.FirstOrDefault();
                var checkmovie = db.movie.FirstOrDefault(m => m.ImdbID == imdb_movie.ImdbID);
                //verify if movie already exists
                if (checkmovie == null)//if not, create new movie on db
                {
                    newmovie.ImdbID = imdb_movie.ImdbID;
                    newmovie.poster = imdb_movie.Poster;
                    newmovie.title = imdb_movie.Title;
                    newmovie.description = imdb_movie.Plot;
                    DateTime tempDate;//convert date of imdb to datetime
                    if (DateTime.TryParse(imdb_movie.Released, out tempDate) == true)
                    {
                        // succeeded ...
                        newmovie.releasedate = tempDate;
                    }
                    else
                    {
                        newmovie.releasedate = DateTime.Now;
                    }
                    //remove min from imdb data 000 min
                    int temptime = 0;
                    int temptime2 = 0;
                    string time = "";

                    for (int i = 0; i < imdb_movie.RunTime.Length; i++)
                    {
                        if (Char.IsDigit(imdb_movie.RunTime[i]))
                            time += imdb_movie.RunTime[i];
                    }

                    //convert data to datetime and sum the minuts
                    if (Int32.TryParse(time, out temptime))
                    {
                        temptime2 = temptime2 + temptime;
                        DateTime dt = DateTime.Now.Date;
                        dt = dt.AddMinutes(temptime2);
                        newmovie.duration = dt;
                    }
                    else
                    {
                        newmovie.duration = DateTime.Now;
                    }
                    //convert string rate to double --- . to , and parse
                    double rate = 0;
                    imdb_movie.imdbRating = imdb_movie.imdbRating.Replace(".", ",");
                    if (double.TryParse(imdb_movie.imdbRating, out rate))
                    {
                        newmovie.ratingImdb = rate;
                    }
                    else
                    {
                        newmovie.ratingImdb = 0;
                    }
                    //create necessary categories in db_ check if already exist
                    List<Category> catlist = new List<Category>();
                    string[] CategoryNames = imdb_movie.Genre.Split(',').Select(sValue => sValue.Trim()).ToArray();
                    foreach (string catn in CategoryNames)
                    {
                        var checkcat = db.categorie.FirstOrDefault(c => c.description == catn);
                        Category newcat = null;
                        if (checkcat == null)
                        {
                            newcat = new Category();
                            newcat.description = catn;
                            db.categorie.Add(newcat);
                        }
                        else
                        {
                            newcat = checkcat;
                        }
                        catlist.Add(newcat);
                    }
                    //add movie to db _ movie does not exist
                    newmovie.categories = catlist;
                    db.movie.Add(newmovie);
                    currentUser.likes.Add(newmovie);

                }
                else
                {
                    //movie exist_ relate with user -- like
                    currentUser.likes.Add(checkmovie);
                }
                db.SaveChanges(); //save db changes
                TempData["successlike"] = imdb_movie.Title + " added with success";
                return RedirectToAction("Search", "FilmesIMDB", new { id = searchValue });//go tlast search
            }
            TempData["successlike"] = "Failed to add movie";
            return RedirectToAction("Search", "FilmesIMDB", new { id = searchValue });//go tlast search
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
