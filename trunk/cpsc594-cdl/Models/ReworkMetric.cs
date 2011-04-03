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
    public class ReworkMetric : PerProductMetric
    {
        public override string Name { get { return "Rework"; } }
        public override int ID { get { return (int)MetricType.Rework; } }

        public ReworkMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Product> products)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Person";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Rework(hours)";

            var productIds = products.Select<Product, int>(x => x.ProductID);

            /***
             * NOTE: Idea for trend...
             * Each product should be a series
             * Xvals are iteratoinID
             ***/

            //This isn't correct YET!
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.Reworks == null || iteration.Reworks.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
				
				foreach (var rw in iteration.Reworks.Where(x => productIds.Contains(x.ProductID)))
                {
                    var existingPoints = series.Points.Where(x => x.XValue == rw.ProductID);
                    if (existingPoints.Count() != 0)
                    {
                        existingPoints.First().YValues[0] += rw.ReworkHours;
                    }
                    else
                    {
                        series.Points.AddXY(rw.ProductID, rw.ReworkHours);
                        series.Points.Last().MarkerSize = 10;
                    }
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(productIds.ToArray<int>()), chart);
        }
    }
}