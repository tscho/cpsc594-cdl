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
        [HttpPost]
        public ActionResult Index(IndexModel model) //data you need is in model
        {
            if (model.ComponentIDs == null)
                ModelState.AddModelError("", "Components Field is empty.");
            if (model.MetricIDs == null)
                ModelState.AddModelError("", "Metrics Field is empty.");

            if (ModelState.IsValid)
            {
                model.Product = DatabaseAccessor.GetProduct(model.ProductID);

                model.ComponentMetrics = new List<PerComponentMetric>();
                model.ProductMetrics = new List<PerProductMetric>();
                IEnumerable<Iteration> iterations = DatabaseAccessor.GetIterations(3);
                foreach (int metricID in model.MetricIDs)
                {
                    Metric metric = Metric.CreateMetricInstanceFromType((MetricType)metricID, iterations);

                    System.Type cls = metric.GetType();
                    if (cls.IsSubclassOf(typeof(PerComponentMetric)))
                        model.ComponentMetrics.Add((PerComponentMetric)metric);
                    else
                        model.ProductMetrics.Add((PerProductMetric)metric);
                }

                model.Components = DatabaseAccessor.GetComponents(model.ComponentIDs);

            }
            return View(model);
        }
    }
}
