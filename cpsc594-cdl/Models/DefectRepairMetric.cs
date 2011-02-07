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
    public class DefectRepairMetric : Metric
    {
        public override string Name { get { return "Defect Repair Rate"; }}
        public override int ID { get { return (int)MetricType.DefectRepairRate;  } }

        public DefectRepairMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.DefectRepairRates == null || iteration.DefectRepairRates.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var repairRate in iteration.DefectRepairRates.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    series.Points.AddXY(repairRate.ComponentID, repairRate.NumberOfResolvedDefects);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = repairRate.Component.ComponentName;
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