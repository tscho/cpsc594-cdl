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
    public class CoverageMetric : IMetric
    {
        public Iteration[] Iterations;

        public string Name { get { return "Code Coverage"; }}

        public String GenerateGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.Coverages == null || iteration.Coverages.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(CoverageMetricData coverage in iteration.Coverages)
                {
                    series.Points.AddXY(coverage.ComponentID, coverage.GetCoverage());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = coverage.Component.ComponentName;
                }
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public string GenerateGraph(string title, Component component)
        {
            return GenerateGraph(title, new Component[] { component });
        }
    }
}