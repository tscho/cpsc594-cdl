using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetricAnalyzer.Portal.Models;

namespace MetricAnalyzer.Portal.Controllers
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
            //Remove 
            /*
            DateTime currentTime = DateTime.Now;
            DateTime lastAccessTime;
            string[] cacheFiles = System.IO.Directory.GetFiles(HttpRuntime.AppDomainAppPath + "Content/cache/");
            foreach (string cacheFile in cacheFiles)
            {
                lastAccessTime = System.IO.File.GetLastAccessTime(cacheFile);
                if ((currentTime - lastAccessTime).TotalSeconds > 30)
                    System.IO.File.Delete(cacheFile);
            }
             */

            IndexModel model = new IndexModel();
            return View(model);
        }
    }
}
