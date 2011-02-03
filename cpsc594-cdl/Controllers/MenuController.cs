using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
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

        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            List<Project> plist = new List<Project>();
            plist.Add(new Project(-1, "Select a Project"));
            plist.AddRange(DatabaseAccessor.GetProjects());
            model.Projects = plist;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            List<Project> plist = new List<Project>();
            plist.Add(new Project(-1, "Select a Project"));
            plist.AddRange(DatabaseAccessor.GetProjects());
            model.Projects = plist;
            if (model.ProjectID == "-1") return View(model);

            List<Component> clist = new List<Component>();
            clist.Add(new Component(-1, -1, "Select All"));
            clist.AddRange(DatabaseAccessor.GetComponents(Convert.ToInt32(model.ProjectID)));
            model.Components = clist;

            return View(model);
        }
    }
}
