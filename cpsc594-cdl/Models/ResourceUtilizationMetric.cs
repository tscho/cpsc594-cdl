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
    public class ResourceUtilizationMetric : Metric
    {
        public override string Name { get { return "Resource Utilization"; } }
        public override int ID { get { return (int)MetricType.ResourceUtilization; } }
        public override bool OverviewOnly { get { return true; } }

        public ResourceUtilizationMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Person";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Resource Utilization(hours)";

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.ResourceUtilizations == null || iteration.ResourceUtilizations.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                
                ResourceUtilization hours = iteration.ResourceUtilizations.Aggregate((x, next) => { x.PersonHours += next.PersonHours; return x; });

                series.Points.AddY(hours.PersonHours);
                series.Points.Last().MarkerSize = 10;
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(new int[] { components.First().ProductID }), chart);
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            return GenerateOverviewGraph(title, new Component[] { component });
        }
    }
}
