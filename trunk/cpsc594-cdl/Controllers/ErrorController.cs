using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cpsc594_cdl.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult DBConnection() {
            ViewData["message"] = "No Database Connection";
            return View("Index");
        }
    }
}
