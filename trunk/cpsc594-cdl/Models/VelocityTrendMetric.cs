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
    public class VelocityTrendMetric : PerProductMetric
    {
        public override string Name { get { return "Velocity Trend"; } }
        public override int ID { get { return (int)MetricType.VelocityTrend; } }

        public VelocityTrendMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, Product product)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Person";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Out Of Scope Work(hours)";

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.OutOfScopeWorks == null || iteration.OutOfScopeWorks.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);

            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(product.ProductID), chart);
        }
    }
}