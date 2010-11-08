using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;

namespace cpsc594_cdl.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["Message"] = "Index Page";
            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            ViewData["PID"] = model.ProjectID;
            if (model.IsSelectProject == "TRUE")
            {
                // Step 1: PID
            }
            else
            {
                // Step 2: PID, COMPS, METRICS
                ViewData["COMPS"] = model.Components;
                ViewData["METRICS"] = model.Metrics;
            }
            return View(model);
        }

        public ActionResult Component()
        {
            return View();
        }

    }
}
