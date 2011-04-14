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
    public class TestEffectivenessMetric : PerProductMetric
    {
        public override string Name { get { return "Value For Tests"; }}
        public override int ID { get { return (int)MetricType.TestEffectiveness;  } }

        public TestEffectivenessMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Product> products)
        {
            var productIds = products.Select<Product, int>(x => x.ProductID);

            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[products.Count()], title = new HighCharts.TextObject("Product") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Tests per defect injected") };

            Dictionary<int, int> xTranslate = new Dictionary<int, int>();
            int i = 0;
            foreach (var product in products)
            {
                xTranslate.Add(product.ProductID, i);
                chart.xAxis.categories[i] = product.ProductName;
                i++;
            }

            var seriesList = new List<HighCharts.Series>();

            HighCharts.Series series;
            List<HighCharts.DataPoint> data;
            foreach (var iteration in Iterations)
            {
                if (iteration.TestEffectivenesses == null || iteration.TestEffectivenesses.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = iteration.IterationLabel };
                data = new List<HighCharts.DataPoint>();

                foreach (var testEffectiveness in iteration.TestEffectivenesses.Where(x => productIds.Contains(x.ProductID)))
                {
                    data.Add(new HighCharts.DataPoint() { name = testEffectiveness.Product.ProductName, x = xTranslate[testEffectiveness.ProductID], y = testEffectiveness.getValue() });
                }

                series.data = data.ToArray();
                seriesList.Add(series);
            }

            chart.series = seriesList.ToArray();
            return chart;
        }
    }
}