using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Models.Repository;

namespace cpsc594_cdl.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Home/
        private ProjectRepository projectRepo;
        private ComponentRepository componentRepo;
        private IterationRepository iterationRepo;

        public MenuController()
        {
            projectRepo = new ProjectRepository();
            componentRepo = new ComponentRepository();
            iterationRepo = new IterationRepository();
        }

        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            List<Project> plist = new List<Project>();
            plist.Add(new Project(-1, "Select a Project"));
            plist.AddRange(projectRepo.getProjects());
            model.Projects = plist;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            List<Project> plist = new List<Project>();
            plist.Add(new Project(-1, "Select a Project"));
            plist.AddRange(projectRepo.getProjects());
            model.Projects = plist;
            if (model.ProjectID == "-1") return View(model);

            List<Component> clist = new List<Component>();
            clist.Add(new Component(-1, -1, "Select All"));
            clist.AddRange(componentRepo.getComponentsForProject(Convert.ToInt32(model.ProjectID)));
            model.Components = clist;

            return View(model);
        }
    }
}
