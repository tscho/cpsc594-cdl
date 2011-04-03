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

        public override string GenerateOverviewGraph(string title, IEnumerable<Product> products)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Iteration ID";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Estimated / Actual";

            var productIds = products.Select<Product, int>(x => x.ProductID);

            Series series;
            foreach (var product in products)
            {
                if (product.VelocityTrends == null || product.VelocityTrends.Count == 0)
                    continue;

                series = new Series(product.ProductName);
                chart.Series.Add(series);

                foreach (var vTrend in product.VelocityTrends.Where(x => iterationIDs.Contains(x.IterationID)))
                {
                    series.Points.AddXY(vTrend.IterationID, vTrend.getValue());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = vTrend.Iteration.IterationLabel;
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(productIds.ToArray<int>()), chart);
        }
    }
}