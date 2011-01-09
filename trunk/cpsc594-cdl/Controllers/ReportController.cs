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
                }
            }
            project.setComponents(Components, model.ComponentIDs);
            return project;
        }

        [HttpPost]
        public ActionResult Index(IndexModel model) //data you need is in model
        {
            if (model.ComponentIDs == null)
                ModelState.AddModelError("", "Components Field is empty.");
            if (model.MetricIDs == null)
                ModelState.AddModelError("", "Metrics Field is empty.");

            Project renderedProject = null;
            if (ModelState.IsValid)
            {
                renderedProject = BuildReportData(model);
                model.Chart1_Base64 = GetChart1(renderedProject);
                model.Chart2_Base64 = GetChart2(renderedProject);
                model.Chart3_Base64 = GetChart3(renderedProject);
                model.Chart4_Base64 = GetChart4(renderedProject);
                model.Components = renderedProject.GetComponents();
            }

            return View(model);
        }

        public String GetChart1(Project project)
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Project Coverage History", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas["ChartArea"].AxisY.Title = "% Code Coverage";
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Maximum = 100;

            // add points to series
            Series series;
            // create series for the project
            series = new Series("Project");
            series.ChartType = SeriesChartType.Area;
            series.IsValueShownAsLabel = false;
            chart.Series.Add(series);
            foreach (Iteration iteration in project.GetTotalIterations())
            {
                series.Points.AddY(iteration.coverage.GetCoverage());
                series.Points.Last().MarkerSize = 10;
                series.Points.Last().AxisLabel = iteration.StartDate.ToString();
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            imageStream.Close();

            return base64_output;
        }


        public String GetChart2(Project project)
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Project Lines of Code History", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisY.Title = "# Lines of Code";
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisX.Interval = 1;

            // add points to series
            Series series;
            // create series for the project
            series = new Series("Project");
            series.ChartType = SeriesChartType.Line;
            series.MarkerStyle = MarkerStyle.Square;
            series.IsValueShownAsLabel = true;
            chart.Series.Add(series);
            foreach (Iteration iteration in project.GetTotalIterations())
            {
                series.Points.AddY(iteration.coverage.GetValue());
                series.Points.Last().MarkerSize = 10;
                series.Points.Last().AxisLabel = iteration.StartDate.ToString();
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public String GetChart3(Project project)
        {
            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Code Coverage History v1", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.Legends.Add("Legend");
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Maximum = 100;

            // create a list of series
            Series series;
            foreach (Component component in project.GetComponents())
            {
                // create a series
                series = new Series(component.Name);
                series.ChartType = SeriesChartType.Line;
                series.MarkerStyle = MarkerStyle.Square;
                chart.Series.Add(series);
                foreach (Iteration iteration in component.Iterations)
                {
                    series.Points.AddY(iteration.coverage.GetCoverage());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate;
                }
            }


            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public String GetChart4(Project project)
        {
            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title title = new Title("Component Code Coverage History v2", Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(title);
            chart.Legends.Add("Legend");
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Maximum = 100;

            // create a list of series
            Series series;
            foreach (var component in project.GetComponents())
            {
                foreach (var iteration in component.Iterations)
                {
                    // create a series
                    if ((series = chart.Series.FindByName(iteration.StartDate)) == null)
                    {
                        series = new Series(iteration.StartDate);
                        chart.Series.Add(series);
                        
                    }
                    series.Points.AddY(iteration.coverage.GetCoverage());
                    //series.Points.AddXY(component.ComponentID, iteration.coverage.GetCoverage());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = component.Name;
                }
            }


            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public String GetChart5()
        {
            componentRepo = new ComponentRepository();
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

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }
    }
}
