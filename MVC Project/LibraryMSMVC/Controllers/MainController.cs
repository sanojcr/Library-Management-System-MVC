using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMSMVC.Controllers
{
    public class MainController : Controller
    {
        // Returns home view.
        public ActionResult Home()
        {
            return View();
        }

        // Returns about view.
        public ActionResult About()
        {
            return View();
        }

        // Returns contact view.
        public ActionResult Contact()
        {
            return View();
        }

        // Returns login view.
        public ActionResult Login()
        {
            return View();
        }
    }
}