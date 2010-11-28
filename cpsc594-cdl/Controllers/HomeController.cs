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

        public HomeController()
        {
            projectRepo = new ProjectRepository();
            componentRepo = new ComponentRepository();
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
            model.Components = componentRepo.getComponentsForProject(Convert.ToInt32(model.ProjectID));
            model.Projects = projectRepo.getProjects();
            return View(model);
        }
    }
}
