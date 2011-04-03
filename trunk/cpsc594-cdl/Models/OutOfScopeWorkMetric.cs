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
    public class OutOfScopeWorkMetric : PerProductMetric
    {
        public override string Name { get { return "Out of Scope Work"; } }
        public override int ID { get { return (int)MetricType.OutOfScopeWork; } }

        public OutOfScopeWorkMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Product>products)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Iteration ID";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Out Of Scope Work(hours)";

            var productIds = products.Select<Product, int>(x => x.ProductID);

            Series series;
            foreach (var product in products)
            {
                if (product.OutOfScopeWorks == null || product.OutOfScopeWorks.Count == 0)
                    continue;

                series = new Series(product.ProductName);
                chart.Series.Add(series);

                foreach (var oos in product.OutOfScopeWorks.Where(x => iterationIDs.Contains(x.IterationID)))
                {
                    var existingPoints = series.Points.Where(x => x.XValue == oos.IterationID);
                    if (existingPoints.Count() > 0)
                    {
                        existingPoints.First().YValues[0] += oos.PersonHours;
                    }
                    else
                    {
                        series.Points.AddXY(oos.IterationID, oos.PersonHours);
                        series.Points.Last().MarkerSize = 10;
                        series.Points.Last().AxisLabel = oos.Iteration.IterationLabel;
                    }
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(productIds.ToArray<int>()), chart);
        }
    }
}
