﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CineChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}