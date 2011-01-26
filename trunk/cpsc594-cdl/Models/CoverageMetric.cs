using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;

namespace cpsc594_cdl.Models
{
    public class CoverageMetric : IMetric
    {
        int linesExecuted;
        int linesCovered;

        public int ComponentID { get; set; }
        public int IterationID { get; set; }
        public DateTime TimeStamp { get; set; }

        public CoverageMetric(int coverageID, int iterationID, int linesExecuted, int linesCovered, DateTime iterationDate)
        {
            this.ComponentID = coverageID;
            this.IterationID = iterationID;
            this.linesExecuted = linesExecuted;
            this.linesCovered = linesCovered;
            this.TimeStamp = iterationDate;
        }

        public int GetValue()
        {
            return linesExecuted;
        }

        public int GetLinesCovered()
        {
            return linesCovered;
        }

        public double GetCoverage()
        {
            return (1.0 * linesExecuted / (linesCovered > 0 ? linesCovered : 1)) * 100; //can't divide by zero! Although lc shouldn't really ever be 0
        }

        public static String GenerateHistogram(IEnumerable<Component> components, string title)
        {
            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title chartTitle = new Title(title, Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(chartTitle);
            chart.Legends.Add("Legend");
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Maximum = 100;

            Series series;
            foreach (var component in components)
            {
                foreach (var iteration in component.Iterations)
                {
                    if ((series = chart.Series.FindByName(iteration.StartDate)) == null)
                    {
                        series = new Series(iteration.StartDate);
                        chart.Series.Add(series);
                        
                    }
                    series.Points.AddY(iteration.coverage.GetCoverage());
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
    }
}