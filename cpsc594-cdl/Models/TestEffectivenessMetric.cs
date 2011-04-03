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
    public class TestEffectivenessMetric : PerProductMetric
    {
        public override string Name { get { return "Test Effectiveness"; }}
        public override int ID { get { return (int)MetricType.TestEffectiveness;  } }

        public TestEffectivenessMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Product> products)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Tests per defect injected";

            var productIds = products.Select<Product, int>(x => x.ProductID);

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.TestEffectivenesses == null || iteration.TestEffectivenesses.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var testEffectiveness in iteration.TestEffectivenesses.Where(x => productIds.Contains(x.ProductID)))
                {
                    series.Points.AddXY(testEffectiveness.ProductID, testEffectiveness.getValue());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = testEffectiveness.Product.ProductName;
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(productIds.ToArray<int>()), chart);
        }
    }
}