using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using MetricAnalyzer.Portal.Models;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Controllers
{
    public class ReportController : Controller
    {
        [HttpPost]
        public ActionResult Index(IndexModel model) //data you need is in model
        {
            var pams = HttpContext.Request.Params;
            if (model.MetricIDs == null)
                ModelState.AddModelError("MetricIDs", "Metrics Field is empty.");
            if (ModelState.IsValid)
            {
                model.ComponentMetrics = new List<PerComponentMetric>();
                model.ProductMetrics = new List<PerProductMetric>();

                IEnumerable<Iteration> iterations;
                if (model.StartIteration < 0 || model.EndIteration < 0)
                    iterations = DatabaseAccessor.GetIterations(2);
                else
                    iterations = DatabaseAccessor.GetIterations(model.StartIteration, model.EndIteration);

                foreach (int metricID in model.MetricIDs)
                {
                    Metric metric = Metric.CreateMetricInstanceFromType((MetricType)metricID, iterations);

                    System.Type cls = metric.GetType();
                    if (cls.IsSubclassOf(typeof(PerComponentMetric)))
                        model.ComponentMetrics.Add((PerComponentMetric)metric);
                    else
                        model.ProductMetrics.Add((PerProductMetric)metric);
                }

                if (model.ComponentMetrics.Count > 0)
                {
                    if (model.ComponentIDs == null)
                        ModelState.AddModelError("ComponentIDs", "Components Field is empty.");
                    if (model.ProductID == null)
                        ModelState.AddModelError("ProductID", "No Product selected");
                }
                if (model.ProductMetrics.Count > 0)
                {
                    if (model.ProductIDs == null)
                        ModelState.AddModelError("ProductIDs", "Products Field is empty.");
                    if (model.StartIteration < 0)
                        ModelState.AddModelError("startIteration", "Starting iteration not set");
                    if (model.EndIteration < 0)
                        ModelState.AddModelError("endIteration", "Ending iteration not set");
                    if (model.EndIteration < model.StartIteration)
                        ModelState.AddModelError("startEndIteration", "Ending iteration before Starting iteration");
                }
            }

            if (ModelState.IsValid)
            {
                if(model.ProductID != null)
                    model.Product = DatabaseAccessor.GetProduct((int)model.ProductID);
                if(model.ProductMetrics.Count > 0)
                    model.Products = DatabaseAccessor.GetProducts(model.ProductIDs);

                if(model.ComponentMetrics.Count > 0)
                    model.Components = DatabaseAccessor.GetComponents(model.ComponentIDs);

            }
            return View(model);
        }

        public JsonResult HighChart(string title, string target, string encodedString, bool? specificComponent)
        {
            Metric metric = Metric.CreateMetricInstanceFromEncodedString(encodedString);

            if(typeof(PerComponentMetric).IsInstanceOfType(metric))
                if(specificComponent != null && (bool)specificComponent)
                    return Json(((PerComponentMetric)metric).GenerateHighChart(title, target, Metric.GetComponentsFromEncodedString(encodedString)));
                else
                    return Json(((PerComponentMetric)metric).GenerateOverviewHighChart(title, target, Metric.GetComponentsFromEncodedString(encodedString)));
            else
                return Json(((PerProductMetric)metric).GenerateHighChart(title, target, Metric.GetProductsFromEncodedString(encodedString)));
        }
    }
}
