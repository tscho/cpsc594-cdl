using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using Util.Database;
using cpsc594_cdl.Models;
using cpsc594_cdl.Models.Repository;
using Component = cpsc594_cdl.Models.Component;
using Iteration = cpsc594_cdl.Models.Iteration;
using Project = cpsc594_cdl.Models.Project;

namespace cpsc594_cdl.Controllers
{
    public class ReportController : Controller
    {

        //public ProjectRepository projectRepo;
        public ComponentRepository componentRepo;
        public IterationRepository iterationRepo;
        public ProjectRepository projectRepo;
        public MetricRepository metricRepo;

        public Project RenderedProject;
        //
        // GET: /Report/

        public ReportController()
        {
            string test;
        }

        public void BuildReportData(IndexModel model)
        {
            componentRepo = new ComponentRepository();
            iterationRepo = new IterationRepository();
            projectRepo = new ProjectRepository();
            metricRepo = new MetricRepository();

            int pid = Int32.Parse(model.ProjectID);


            RenderedProject = projectRepo.getProject(pid);
            RenderedProject.Components = componentRepo.getComponentsForProject(pid);

            DateTime startDate = Convert.ToDateTime(model.StartMonth+"/"+model.StartDay+"/"+model.StartYear);
            List<Iteration> iterationList = iterationRepo.getIterationsForComponent(startDate);

            foreach (Component component in RenderedProject.Components)
            {
                component.Iterations = iterationList;
                //Loop through iterations and get metrics associated to the iteration & calculate
                foreach (Iteration currIteration in component.Iterations)
                {

                    CoverageMetric componentCodeCoverage = metricRepo.getCoverage(currIteration.iterationID,
                                                                              component.ComponentID,
                                                                              currIteration.EndDate);
                    currIteration.coverage = componentCodeCoverage;
                }
            }
        }

        [HttpPost]
        public ActionResult Index(IndexModel model) //data you need is in model
        {
            BuildReportData(model);
            return View();
        }

        public FileResult GetChart1(int pid, string str_components, string str_metrics)
        {
            List<double> data = componentRepo.getCodeCoverage(pid);

            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Coverage History", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas["ChartArea"].AxisY.Title = "% Code Coverage";
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisX.Interval = 1;

            // create a series
            var series = new Series("Series");
            series.ChartType = SeriesChartType.FastLine;
            chart.Series.Add(series);

            // add points to series
            int i = 1;
            foreach (double value in data)
            {
                series.Points.AddY(value);
                series.Points.Last().AxisLabel = "Date " + (i++);
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }

        public FileResult GetChart2(int pid, string str_components, string str_metrics)
        {
            List<int> data = componentRepo.getSample();

            string[] list;
            List<int> components = new List<int>();
            List<string> component_names = new List<string>();
            List<int> metrics = new List<int>();

            // Convert components text into array
            int tmp;
            list = str_components.Split(',');
            foreach (string str in list)
            {
                tmp = Convert.ToInt32(str);
                components.Add(tmp);
                component_names.Add(componentRepo.getName(tmp));
            }

            // Convert metrics text into array
            list = str_metrics.Split(',');
            foreach (string str in list)
                metrics.Add(Convert.ToInt32(str));

            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Status 1", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.Interval = 1;

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
            List<int> data = componentRepo.getSample();
            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Status 2", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.Interval = 1;

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
