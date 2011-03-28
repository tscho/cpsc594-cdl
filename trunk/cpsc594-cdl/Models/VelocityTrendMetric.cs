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
            chart.ChartAreas[0].AxisX.Title = "Contract ID";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Estimated / Actual";

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.VelocityTrends == null || iteration.VelocityTrends.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);

                foreach (var vTrend in iteration.VelocityTrends.Where(x => x.ProductID == product.ProductID))
                {
                    series.Points.AddXY(vTrend.ContractID, vTrend.getValue());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = vTrend.ContractID.ToString();
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(product.ProductID), chart);
        }
    }
}