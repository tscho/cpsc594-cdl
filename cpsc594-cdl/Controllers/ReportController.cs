using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using cpsc594_cdl.Models;
using cpsc594_cdl.Models.Repository;

namespace cpsc594_cdl.Controllers
{
    public class ReportController : Controller
    {
        public ProjectRepository projectRepo;
        public ComponentRepository componentRepo;
        //
        // GET: /Report/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public FileResult GetChart1(int pid, string str_components, string str_metrics)
        {
            List<int> data = Models.StaticModel.createStaticData();
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Coverage History", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas["ChartArea"].AxisY.Title = "% Code Coverage";

            // create a series
            var series1 = new Series("Series");
            series1.ChartType = SeriesChartType.FastLine;
            chart.Series.Add(series1);

            // add points to series 1
            int i = 1;
            foreach (int value in data)
            {
                series1.Points.AddY(value);
                series1.Points.Last().AxisLabel = "Date " + (i++);
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }

        public FileResult GetChart2(int pid, string str_components, string str_metrics)
        {
            List<int> data = Models.StaticModel.createStaticData();
            Chart chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Status 1", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");

            // create a series
            var series1 = new Series("Confirmed (15 sep)");
            chart.Series.Add(series1);
            var series2 = new Series("Confirmed (04 oct)");
            chart.Series.Add(series2);

            // add points to series 1
            int i = 1;
            foreach (int value in data)
            {
                series1.Points.AddY(value);
                series2.Points.AddY(value+10);
                series1.Points.Last().AxisLabel = "Component " + (i++);
            }

            chart.Legends.Add("Legend");

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }

        public FileResult GetChart3(int pid, string str_components, string str_metrics)
        {
            List<int> data = Models.StaticModel.createStaticData();
            Chart chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Status 2", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");

            // create a series
            var series1 = new Series("Resolved (sep 15)");
            chart.Series.Add(series1);
            var series2 = new Series("Verified (sep 15)");
            chart.Series.Add(series2);
            var series3 = new Series("Resolved (oct 4)");
            chart.Series.Add(series3);
            var series4 = new Series("Verified (oct 4)");
            chart.Series.Add(series4);

            // add points to series 1
            int i = 1;
            foreach (int value in data)
            {
                series1.Points.AddY(value);
                series2.Points.AddY(value + 10);
                series3.Points.AddY(value + 15);
                series4.Points.AddY(value/2);
                series1.Points.Last().AxisLabel = "Component " + (i++);
            }

            chart.Legends.Add("Legend");

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }
    }
}
