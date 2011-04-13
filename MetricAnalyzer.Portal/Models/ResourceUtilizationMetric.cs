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

            return ChartImageCache.GetImageCache().SaveChartImage(this.StringEncode(productIds.ToArray<int>()), chart);
        }

        public override HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Product> products)
        {
            var productIds = products.Select<Product, int>(x => x.ProductID);

            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[Iterations.Count()], title = new HighCharts.TextObject("Iteration ID") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Resource Utilization(hours)") };

            //Mapping productIDs to categories
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
                if (product.ResourceUtilizations == null || product.ResourceUtilizations.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = product.ProductName };
                data = new List<HighCharts.DataPoint>();

				foreach (var ru in product.ResourceUtilizations.Where(x => iterationIDs.Contains(x.IterationID)))
                {
                    var existingPoints = data.Where(x => x.x == xTranslate[ru.IterationID]);
                    if (existingPoints.Count() > 0)
                    {
                        existingPoints.FirstOrDefault().y += ru.PersonHours;
                    }
                    else
                    {
                        data.Add(new HighCharts.DataPoint() { name = product.ProductName, x = xTranslate[ru.IterationID], y = ru.PersonHours });
                    }
                }

                series.data = data.ToArray();
                seriesList.Add(series);
            }

            chart.series = seriesList.ToArray();
            return chart;
        }
    }
}
