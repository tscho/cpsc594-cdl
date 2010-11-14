using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using cpsc594_cdl.Models;
using System.IO;

namespace cpsc594_cdl.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public static int pid;
        public static IEnumerable<int> components;
        public static IEnumerable<int> metrics;

        public ActionResult Index()
        {
            ViewData["Message"] = "Index Page";
            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            if (model.IsSelectProject == "FALSE" && model.Components != null && model.Metrics != null)
            {
                // Step 2: Get project id, components, metrics
                pid = Convert.ToInt32(model.ProjectID);
                components = model.Components;
                metrics = model.Metrics;
                return RedirectToAction("Component", "Home");
            } else {
                // Step 1 OR Error on input: Get project id
                ViewData["PID"] = model.ProjectID;
            }
            return View(model);
        }

        public ActionResult Component()
        {
            ViewData["PID"] = pid;
            ViewData["Components"] = components;
            ViewData["Metrics"] = metrics;

            return View();
        }

        public FileResult GetChart()
        {
            List<int> data  = Models.StaticModel.createStaticData();
            Chart chart_1 = new Chart();
            chart_1.Width = 800;
            chart_1.Height = 400;
            chart_1.RenderType = RenderType.ImageTag;
            chart_1.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Coverage History", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart_1.Titles.Add(title);
            chart_1.ChartAreas.Add("Chart Area");

            // create a series
            var series1 = new Series("Series");
            series1.ChartType = SeriesChartType.FastLine;
            chart_1.Series.Add(series1);

            // add points to series 1
            foreach (int value in data)
            {
                series1.Points.AddY(value);
            }

            MemoryStream imageStream = new MemoryStream();
            chart_1.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }
    }
}
