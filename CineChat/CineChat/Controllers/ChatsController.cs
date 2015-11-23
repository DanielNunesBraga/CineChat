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
using System.Data.Entity.Infrastructure;

namespace CineChat.Controllers
{
    public class ChatsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chats
        [Authorize]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View(db.chat.ToList());
            }
            Category category = db.categorie.FirstOrDefault(c => c.ID == id);
            ViewBag.selectedcat = "- " + category.description;
            return View(db.chat.ToList().Where(ch => ch.category.ID == id));
        }

        //get channels i am loged in and possibility to exit that channel
        [Authorize]
        public ActionResult ListLoged()
        {
            string CurrentUserID = User.Identity.GetUserId();
            return View(db.chatuser.ToList().Where(x => x.user.Id == CurrentUserID));
        }

        [Authorize]
        public ActionResult MyChannel()
        {
            string CurrentUserID = User.Identity.GetUserId();
            var CurrentUser = db.Users.FirstOrDefault(u => u.Id == CurrentUserID);
            return View("Index", CurrentUser.chat.ToList());
        }


        // GET: Chats/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            Chat chat = db.chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }

            if (db.chatuser.FirstOrDefault(ch => ch.user.Id == currentUser.Id && ch.chat.ID == id) == null)
            {
                if (string.IsNullOrEmpty(chat.password))
                {
                    ChatUsers chatuser = new ChatUsers();
                    db.chatuser.Add(chatuser);
                    currentUser.logeduser.Add(chatuser);
                    chat.logeduser.Add(chatuser);
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("chatpwd/" + chat.ID);
                }
            }
            return View(chat);
        }

        [Authorize]
        public ActionResult chatpwd(int? id)
        {
            ViewBag.chatID = id;
            var chat = db.chat.FirstOrDefault(x => x.ID == id);
            if (chat != null)
            {
                ViewBag.chatTitle = chat.title;
            }
            return View();
        }

        [Authorize]
        public ActionResult ChatLogout(int? id)
        {
            if(id != null)
            {
                string CurrentUserId = User.Identity.GetUserId();
                var CurrentLogedUser = db.chatuser.FirstOrDefault(cu => cu.chat.ID == id && cu.user.Id == CurrentUserId);
                if (CurrentLogedUser != null)
                {
                    db.chatuser.Remove(CurrentLogedUser);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chatpwd(int? id, string password)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            else if (string.IsNullOrEmpty(password) || password != chat.password)
            {
                ViewBag.pwderror = "Invalid password";
                return RedirectToAction("chatpwd/" + chat.ID);
            }
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ChatUsers chatuser = new ChatUsers();
            db.chatuser.Add(chatuser);
            currentUser.logeduser.Add(chatuser);
            chat.logeduser.Add(chatuser);
            db.SaveChanges();
            return RedirectToAction("Details/" + id);
        }



        // GET: Chats/Create
        [Authorize]
        public ActionResult Create()
        {
            PopulateCategoryDropDownList();
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,title,password")] Chat chat, int? category)
        {
            try {
                if (category != null)
                {
                    if (ModelState.IsValid)
                    {
                        var currentCategory = db.categorie.FirstOrDefault(ct => ct.ID == category);
                        if(currentCategory == null)
                        {
                            PopulateCategoryDropDownList();
                            return View(chat);
                        }
                        chat.category = currentCategory;
                        db.chat.Add(chat);
                        string currentUserId = User.Identity.GetUserId();
                        var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                        currentUser.chat.Add(chat);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateCategoryDropDownList(chat.category);
            return View(chat);
        }

        // GET: Chats/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,title,password")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chat);
        }

        // GET: Chats/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.chat.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            Chat chat = db.chat.Find(id);
            currentUser.chat.Remove(chat);
            foreach(var loggeduser in db.chatuser.Where(cu => cu.chat.ID == id))
            {
                db.chatuser.Remove(loggeduser);
            }
            foreach(var post in db.post.Where(p => p.chat.ID == id))
            {
                db.post.Remove(post);
            }
            db.chat.Remove(chat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //create a category dropdown list
        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from d in db.categorie
                                   orderby d.description
                                   select d;
            ViewBag.category = new SelectList(categoriesQuery, "ID", "description", selectedCategory);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(int? id, string message)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Chats");
            }
            Post post = new Models.Post(); ;
            string currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            var currentchat = db.chat.FirstOrDefault(y => y.ID == id);
            post.message = message;            
            post.time = DateTime.Now;
            db.post.Add(post);
            currentUser.posts.Add(post);
            currentchat.posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Details/" + id, "Chats");
        }

        [Authorize]
        public ActionResult PostDelete(int? id)
        {
            if (id != null)
            {
                Post post = db.post.Find(id);
                int chatid = post.chat.ID;
                string currentUserId = User.Identity.GetUserId();
                if (post.user.Id == currentUserId || post.chat.admin.Id == currentUserId)
                {
                    db.post.Remove(post);
                    db.SaveChanges();
                }
                return RedirectToAction("Details/" + chatid, "Chats");
            }
            return RedirectToAction("Index", "Chats");
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
