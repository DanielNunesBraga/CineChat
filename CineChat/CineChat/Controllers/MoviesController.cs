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

namespace CineChat.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        [Authorize]
        public ActionResult Index()
        {
            return View(db.movie.ToList());
        }

        [Authorize]
        public ActionResult MyMovies()
        {
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(currentUser.likes.ToList());
        }

        // GET: Movies/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        [Authorize]
        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            else
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                //add movie to current user
                currentUser.likes.Add(movie);
                db.SaveChanges();
            }
            return RedirectToAction("MyMovies");
        }


        // GET: Movies/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ImdbID,title,releasedate,duration,ratingImdb,description")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                //get current user
                string currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                //add movie to db
                db.movie.Add(movie);

                //add movie to current user
                currentUser.likes.Add(movie);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Movie movie = db.movie.Find(id);
            Movie movie = db.movie.Include(i => i.categories).Where(i => i.ID == id ).Single() ;

            PopulateCategoryData(movie);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


        // movie edit get list of categories
        private void PopulateCategoryData(Movie movie)
        {
            var allCategory = db.categorie;
            var moviecategory = new HashSet<int>(movie.categories.Select(c => c.ID));
            var viewModel = new List<AssignedCategoryData>();
            foreach (var category in allCategory)
            {
                viewModel.Add(new AssignedCategoryData
                {
                    CategoryID = category.ID,
                    Description = category.description,
                    Assigned = moviecategory.Contains(category.ID)
                });
            }
            ViewBag.Categories = viewModel;
        }



        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var movieToUpdate = db.movie
               .Include(i => i.categories).Where(i => i.ID == id).Single();

            if (TryUpdateModel(movieToUpdate, "",
            new string[] { "ImdbID", "title", "releasedate", "duration", "ratingImdb", "description" }))
            {
                try
                {


                    UpdateMovieCategory(selectedCategories, movieToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateCategoryData(movieToUpdate);
            return View(movieToUpdate);
        }

        //---------------Edit POST Controller  modify categories--------------------------------
        private void UpdateMovieCategory(string[] selectedCategories, Movie movieToUpdate)
        {
            if (selectedCategories == null)
            {
                movieToUpdate.categories = new List<Category>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var moviecategory = new HashSet<int>
                (movieToUpdate.categories.Select(c => c.ID));
            foreach (var category in db.categorie)
            {
                if (selectedCategoriesHS.Contains(category.ID.ToString()))
                {
                    if (!moviecategory.Contains(category.ID))
                    {
                        movieToUpdate.categories.Add(category);
                    }
                }
                else
                {
                    if (moviecategory.Contains(category.ID))
                    {
                        movieToUpdate.categories.Remove(category);
                    }
                }
            }
        }


        // GET: Movies/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.movie.Find(id);
            db.movie.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
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
