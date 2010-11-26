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
        public static int pid;
        public static IEnumerable<int> components;
        public static IEnumerable<int> metrics;
        public static string start_date;

        private ProjectRepository projectRepo;
        private ComponentRepository componentRepo;

        public HomeController()
        {
            projectRepo = new ProjectRepository();
            componentRepo = new ComponentRepository();
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Index Page";
            IndexModel model = new IndexModel();
            model.Projects = projectRepo.getProjects();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            if (model.IsSelectProject == "FALSE" && model.ComponentIDs != null && model.Metrics != null)
            {
                // Step 2: Get project id, components, metrics, start date
                pid = Convert.ToInt32(model.ProjectID);
                components = model.ComponentIDs;
                metrics = model.Metrics;
                start_date = model.StartDate;
                return RedirectToAction("Component", "Home");
            } else {
                // Step 1 OR Error on input: Get project id
                ViewData["PID"] = model.ProjectID;
                model.Components = componentRepo.getComponentsForProject(Convert.ToInt32(model.ProjectID));
            }
            model.Projects = projectRepo.getProjects();
            return View(model);
        }

        public ActionResult Component()
        {
            ViewData["PID"] = pid;
            ViewData["StartDate"] = start_date;

            IEnumerator<int> list;
            string text;

            // Convert components array into text
            list = components.GetEnumerator();
            text = "";
            while (list.MoveNext())
                text += "," + list.Current;
            ViewData["Components"] = text.Substring(1);

            // Convert metrics array into text
            list = metrics.GetEnumerator();
            text = "";
            while (list.MoveNext())
                text += "," + list.Current;
            ViewData["Metrics"] = text.Substring(1);

            return View();
        }
    }
}
