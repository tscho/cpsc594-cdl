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
        private ProjectRepository projectRepo;
        private ComponentRepository componentRepo;
        private IterationRepository iterationRepo;

        public HomeController()
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
            model.Components = componentRepo.getComponentsForProject(Convert.ToInt32(model.ProjectID));
            //model.Iterations = iterationRepo.getStartDatesForComponent(12);

            return View(model);
        }
    }
}
