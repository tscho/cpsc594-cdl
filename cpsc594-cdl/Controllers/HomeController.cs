using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
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
            if (model.Components != null && model.Metrics != null)
            {
                // Step 2: Get project id, components, metrics
                pid = Convert.ToInt32(model.ProjectID);
                components = model.Components;
                metrics = model.Metrics;
                return RedirectToAction("Component", "Home");
            }
            else
            {
                // Step 1: Get project id
                if (model.ProjectID != "0")
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
            System.Web.UI.DataVisualization.Charting.Chart Chart2 = new System.Web.UI.DataVisualization.Charting.Chart();
            Chart2.Width = 412;
            Chart2.Height = 296;
            Chart2.RenderType = System.Web.UI.DataVisualization.Charting.RenderType.ImageTag;
            Chart2.Palette = ChartColorPalette.BrightPastel;
            Title t = new Title("Sample Chart", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
            Chart2.Titles.Add(t);
            Chart2.ChartAreas.Add("Series 1");
            // create a couple of series   
            Chart2.Series.Add("Series 1");
            Chart2.Series.Add("Series 2");
            // add points to series 1   
            foreach (int value in data)
            {
                Chart2.Series["Series 1"].Points.AddY(value);
            }
            // add points to series 2   
            foreach (int value in data)
            {
                Chart2.Series["Series 2"].Points.AddY(value + 1);
            }
            Chart2.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            Chart2.BorderlineWidth = 2;
            Chart2.BorderColor = System.Drawing.Color.Black;
            Chart2.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart2.BorderWidth = 2;
            Chart2.Legends.Add("Legend1");
            MemoryStream imageStream = new MemoryStream();
            Chart2.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }
    }
}
