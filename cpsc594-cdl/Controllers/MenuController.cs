using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Infrastructure;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Home/
        public MenuController()
        {
        }

        [DatabaseRequired]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            List<Project> plist = new List<Project>();
            plist.Add(new Project() { ProjectID = -1, ProjectName = "Select a Project" });
            plist.AddRange(DatabaseAccessor.GetProjects());
            model.Projects = plist;

            return View(model);
        }

        [HttpPost]
        [DatabaseRequired]
        public ActionResult Index(IndexModel model)
        {
            List<Project> plist = new List<Project>();
            plist.Add(new Project() { ProjectID = -1, ProjectName = "Select a Project" });
            plist.AddRange(DatabaseAccessor.GetProjects());
            model.Projects = plist;
            if (model.ProjectID == -1) return View(model);

            List<Component> clist = new List<Component>();
            clist.Add(new Component() { ComponentID = -1, ProjectID = -1, ComponentName = "Select All" });
            clist.AddRange(DatabaseAccessor.GetComponents(Convert.ToInt32(model.ProjectID)));
            model.Components = clist;

            model.MetricIDs = new[] { 0, 1, 2, 3 };

            return View(model);
        }
    }
}
