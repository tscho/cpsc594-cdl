using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using cpsc594_cdl.Models;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Controllers
{
    public class ReportController : Controller
    {
        /*
        public ComponentRepository componentRepo;
        public IterationRepository iterationRepo;
        public ProjectRepository projectRepo;
        public MetricRepository metricRepo;

        public ReportController()
        {
            componentRepo = new ComponentRepository();
            iterationRepo = new IterationRepository();
            projectRepo = new ProjectRepository();
            metricRepo = new MetricRepository();
        }

        private Project BuildReportData(IndexModel model)
        {
            int pid = Int32.Parse(model.ProjectID);

            var project = projectRepo.getProject(pid);
            model.ProjectName = project.Name;
            List<Component> Components = componentRepo.getComponentsForProject(pid);

            List<Iteration> iterationList = iterationRepo.getIterations(12);
            
            foreach (Component component in Components)
            {
                component.Iterations = new List<Iteration>();
                foreach (Iteration iteration in iterationList)
                    component.Iterations.Add(iteration.clone());
                //Loop through iterations and get metrics associated to the iteration & calculate
                foreach (Iteration currIteration in component.Iterations)
                {
                    CoverageMetric componentCodeCoverage = metricRepo.getCoverage(currIteration.iterationID,
                                                                              component.ComponentID,
                                                                              currIteration.EndDate);
                    currIteration.coverage = componentCodeCoverage;
                    DefectInjectionMetric componentDefectInjection = metricRepo.getDefectInjection(currIteration.iterationID,
                                                                              component.ComponentID,
                                                                              currIteration.EndDate);
                    currIteration.defectInjection = componentDefectInjection;
                }
            }
            project.setComponents(Components, model.ComponentIDs);
            return project;
        }
         */

        [HttpPost]
        public ActionResult Index(IndexModel model) //data you need is in model
        {
            if (model.ComponentIDs == null)
                ModelState.AddModelError("", "Components Field is empty.");
            if (model.MetricIDs == null)
                ModelState.AddModelError("", "Metrics Field is empty.");

            if (ModelState.IsValid)
            {
                model.Metrics = new List<Metric>();
                IEnumerable<Iteration> iterations = DatabaseAccessor.GetIterations(3);
                foreach (int metricID in model.MetricIDs)
                {
                    Metric metric = null;
                    switch ((MetricType)metricID)
                    {
                        case MetricType.Coverage:
                            metric = new CoverageMetric(iterations);
                            break;
                        case MetricType.DefectInjectionRate:
                            metric = new DefectInjectionMetric(iterations);
                            break;
                        case MetricType.DefectRepairRate:
                            metric = new DefectRepairMetric(iterations);
                            break;
                    }
                    model.Metrics.Add(metric);
                }

                model.Components = DatabaseAccessor.GetComponents(model.ComponentIDs);

            }
            return View(model);
        }
    }
}
