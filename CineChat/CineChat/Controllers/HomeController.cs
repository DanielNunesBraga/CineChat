using CineChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CineChat.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CineChat.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.counts = "";
            List<Movie> movieliked = new List<Movie>();
            var query = db.movie.ToList().Select(x => new
            {
                movieID = x.ID,
                like_count = x.likes.Count
            }).OrderByDescending(x => x.like_count).Take(5);
            foreach (var movie in query)
            {
                Movie addmv = new Movie();
                addmv = db.movie.FirstOrDefault(m => m.ID == movie.movieID);
                movieliked.Add(addmv);
            }
            IEnumerable<CineChat.MyFilmsAPIModels.inTheater> inTheaters = CineChat.Controllers.MyFilmsApiComingSoonController.apiSearch();
            ViewBag.inTheaters = inTheaters.ToList();          
            return View(movieliked);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyList()
        {
            ViewBag.Message = "Your search page.";

            return View();
        }

        public ActionResult MovieDetails()
        {
            ViewBag.Message = "Your search page.";

            return View();
        }
        public ActionResult TopRated()
        {
            ViewBag.counts = "";
            List<Movie> movieliked = new List<Movie>();
            var query = db.movie.ToList().Select(x => new
            {
                movieID = x.ID,
                like_count = x.likes.Count
            }).OrderByDescending(x => x.like_count).Take(40);
            foreach (var movie in query)
            {
                Movie addmv = new Movie();
                addmv = db.movie.FirstOrDefault(m => m.ID == movie.movieID);
                movieliked.Add(addmv);
            }
            return View(movieliked);
        }

        public ActionResult Chat()
        {
            return View();
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