﻿using System;
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
        public static int pid;
        public static IEnumerable<int> components;
        public static IEnumerable<int> metrics;

        public ActionResult Index()
        {
            ViewData["Message"] = "Index Page";
            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            if (model.IsSelectProject == "FALSE" && model.Components != null && model.Metrics != null)
            {
                // Step 2: Get project id, components, metrics
                pid = Convert.ToInt32(model.ProjectID);
                components = model.Components;
                metrics = model.Metrics;
                return RedirectToAction("Component", "Home");
            } else {
                // Step 1 OR Error on input: Get project id
                ViewData["PID"] = model.ProjectID;
            }
            return View(model);
        }

        public ActionResult Component()
        {
            ViewData["PID"] = pid;
            ViewData["Components"] = components;
            ViewData["Metrics"] = metrics;

            return View();
        }
    }
}
