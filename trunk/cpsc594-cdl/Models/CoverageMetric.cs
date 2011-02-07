using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using cpsc594_cdl.Common.Models;
using cpsc594_cdl.Infrastructure;

namespace cpsc594_cdl.Models
{
    public class CoverageMetric : Metric
    {
        public override string Name { get { return "Code Coverage"; }}
        public override int ID { get { return (int)MetricType.Coverage;  } }

        public CoverageMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title, true);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Components";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Coverage %";

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.Coverages == null || iteration.Coverages.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var coverage in iteration.Coverages.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    series.Points.AddXY(coverage.ComponentID, coverage.GetCoverage());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = coverage.Component.ComponentName;
                    series.Points.Last().Label = ""+((int)coverage.GetCoverage());
                }
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            return GenerateOverviewGraph(title, new Component[] { component });
        }
    }
}