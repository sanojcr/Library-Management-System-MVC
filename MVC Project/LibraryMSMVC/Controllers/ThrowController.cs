using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryMSMVC.Controllers
{
    public class ThrowController : Controller
    {
        // GET: Throw
        public ActionResult Index()
        {
            throw new Exception();
        }
    }
}