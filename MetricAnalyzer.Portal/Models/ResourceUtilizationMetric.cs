using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using MetricAnalyzer.Common.Models;
using MetricAnalyzer.Portal.Infrastructure;

namespace MetricAnalyzer.Portal.Models
{
    public class ResourceUtilizationMetric : PerProductMetric
    {
        public override string Name { get { return "Resource Utilization"; } }
        public override int ID { get { return (int)MetricType.ResourceUtilization; } }

        public ResourceUtilizationMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Product> products)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Iteration ID";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Resource Utilization(hours)";

            var productIds = products.Select<Product, int>(x => x.ProductID);

            Series series;
            foreach (var product in products)
            {
                if (product.ResourceUtilizations == null || product.ResourceUtilizations.Count == 0)
                    continue;

                series = new Series(product.ProductName);
                chart.Series.Add(series);
                
                foreach (var ru in product.ResourceUtilizations.Where(x => iterationIDs.Contains(x.IterationID)))
                {
                    var existingPoints = series.Points.Where(x => x.XValue == ru.IterationID);
                    if (existingPoints.Count() != 0)
                    {
                        existingPoints.First().YValues[0] += ru.PersonHours;
                    }
                    else
                    {
                        series.Points.AddXY(ru.IterationID, ru.PersonHours);
                        series.Points.Last().MarkerSize = 10;
                        series.Points.Last().AxisLabel = ru.Iteration.IterationLabel;
                    }
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(productIds.ToArray<int>()), chart);
        }
    }
}
