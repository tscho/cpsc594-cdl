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
        public ProducRepository productRepo;
        public MetricRepository metricRepo;

        public ReportController()
        {
            componentRepo = new ComponentRepository();
            iterationRepo = new IterationRepository();
            productRepo = new ProductRepository();
            metricRepo = new MetricRepository();
        }

        private Product BuildReportData(IndexModel model)
        {
            int pid = Int32.Parse(model.ProductID);

            var product = productRepo.getProduct(pid);
            model.ProductName = product.Name;
            List<Component> Components = componentRepo.getComponentsForProduct(pid);

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
            product.setComponents(Components, model.ComponentIDs);
            return product;
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
                        case MetricType.TestEffectiveness:
                            metric = new TestEffectivenessMetric(iterations);
                            break;
                        case MetricType.OutOfScopeWork:
                            metric = new OutOfScopeWorkMetric(iterations);
                            break;
                        case MetricType.ResourceUtilization:
                            metric = new ResourceUtilizationMetric(iterations);
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
