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
            model.Projects = projectRepo.getProjects();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            model.Projects = projectRepo.getProjects();
            List<Component> list = new List<Component>();
            list.Add(new Component(-1, -1, "Select All"));
            list.AddRange(componentRepo.getComponentsForProject(Convert.ToInt32(model.ProjectID)));
            model.Components = list;

            return View(model);
        }
    }
}
