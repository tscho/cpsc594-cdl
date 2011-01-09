using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Models.Repository;

namespace cpsc594_cdl.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            IndexModel model = new IndexModel();
            return View(model);
        }
    }
}
