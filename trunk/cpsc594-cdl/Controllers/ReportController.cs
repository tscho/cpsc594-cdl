using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Models.Repository;

namespace cpsc594_cdl.Controllers
{
    public class ReportController : Controller
    {
        public ProjectRepository projectRepo;
        public ComponentRepository componentRepo;
        //
        // GET: /Report/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}
