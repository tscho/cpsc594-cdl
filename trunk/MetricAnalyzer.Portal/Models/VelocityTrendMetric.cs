using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using MetricAnalyzer.Common.Models;
using MetricAnalyzer.Portal.Infrastructure;

namespace MetricAnalyzer.Portal.Models
{
    public class VelocityTrendMetric : PerProductMetric
    {
        public override string Name { get { return "Velocity Trend"; } }
        public override int ID { get { return (int)MetricType.VelocityTrend; } }

        public VelocityTrendMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Product> products)
        {
            var productIds = products.Select<Product, int>(x => x.ProductID);

            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[Iterations.Count()], title = new HighCharts.TextObject("Iteration ID") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Estimated / Actual") };

            Dictionary<int, int> xTranslate = new Dictionary<int, int>();
            int i = 0;
            foreach (var iteration in Iterations)
            {
                xTranslate.Add(iteration.IterationID, i);
                chart.xAxis.categories[i] = iteration.IterationLabel;
                i++;
            }

            var seriesList = new List<HighCharts.Series>();

            HighCharts.Series series;
            List<HighCharts.DataPoint> data;
            foreach (var product in products)
            {
                if (product.VelocityTrends == null || product.VelocityTrends.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = product.ProductName };
                data = new List<HighCharts.DataPoint>();

				foreach (var vTrend in product.VelocityTrends.Where(x => iterationIDs.Contains(x.IterationID)))
                {
                    data.Add(new HighCharts.DataPoint() { name = product.ProductName, x = xTranslate[vTrend.IterationID], y = vTrend.getValue() });
                }

                series.data = data.ToArray();
                seriesList.Add(series);
            }

            chart.series = seriesList.ToArray();
            return chart;
        }
    }
}