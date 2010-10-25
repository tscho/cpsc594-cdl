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
                // Step 2: PID, CM_Array(components, metrics)
                //ViewData["Message"] = model.CM_Array;
                ArrayList components = new ArrayList();
                ArrayList metrics = new ArrayList();
                string[] r_comp = model.CM_Array.Split(",".ToCharArray());
                for (int i = 0; i < r_comp.Length - 1; i++)
                {
                    if (r_comp[i].Substring(0, 1) == "c")
                        components.Add(Convert.ToInt32(r_comp[i].Substring(2)));
                    else
                        metrics.Add(Convert.ToInt32(r_comp[i].Substring(2)));
                }
            }
            return View(model);
        }

        public ActionResult Component()
        {
            return View();
        }

    }
}
