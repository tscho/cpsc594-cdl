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
    public class DefectInjectionRateMetric : PerComponentMetric
    {
        public override string Name { get { return "Defect Injection Rate";  } }
        public override int ID { get { return (int)MetricType.DefectInjectionRate;  } }

        public DefectInjectionRateMetric(IEnumerable<Iteration> iterations) : base(iterations) {}

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Components";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Total Number of Defects\n(High, Medium, Low Defects)";

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.DefectInjectionRates == null || iteration.DefectInjectionRates.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var injectionRate in iteration.DefectInjectionRates.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    int value = (int)(injectionRate.GetValue());
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = injectionRate.Component.ComponentName;
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.StringEncode(componentIds.ToArray()), chart);
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            //return GenerateOverviewGraph(title, new Component[] { component });
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Iterations";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Number of Defects";

            Series series;
            double value;
            // High
            series = new Series("High");
            chart.Series.Add(series);
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    value = (int)injectionRate.NumberOfHighDefects;
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate.ToShortDateString();
                }
            }
            // Medium
            series = new Series("Medium");
            chart.Series.Add(series);
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    value = (int)injectionRate.NumberOfMediumDefects;
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate.ToShortDateString();
                }
            }
            // Low
            series = new Series("Low");
            chart.Series.Add(series);
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    value = (int)injectionRate.NumberOfLowDefects;
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate.ToShortDateString();
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.StringEncode(component.ComponentID), chart);
        }

        public override HighCharts.HighChart GenerateOverviewHighChart(string title, string target, IEnumerable<Component> components)
        {
            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[components.Count()], title = new HighCharts.TextObject("Components") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Total Number of Defects\n(High, Medium, Low Defects)") };

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);

            Dictionary<int, int> xTranslate = new Dictionary<int, int>();
            int i = 0;
            foreach (var component in components)
            {
                xTranslate.Add(component.ComponentID, i);
                chart.xAxis.categories[i] = component.ComponentName;
                i++;
            }

            var seriesList = new List<HighCharts.Series>();

            HighCharts.Series series;
            List<HighCharts.DataPoint> data;
            foreach (var iteration in Iterations)
            {
                if (iteration.DefectInjectionRates == null || iteration.DefectInjectionRates.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = iteration.IterationLabel };
                data = new List<HighCharts.DataPoint>();

                foreach(var injectionRate in iteration.DefectInjectionRates.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { name = injectionRate.Component.ComponentName, x = xTranslate[injectionRate.ComponentID], y = injectionRate.GetValue() });
                }

                series.data = data.ToArray();
                seriesList.Add(series);
            }

            chart.series = seriesList.ToArray();
            return chart;
        }

        public override HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Component> components)
        {
            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[Iterations.Count()], title = new HighCharts.TextObject("Iterations") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Number of Defects") };

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);

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

            //High
            var component = components.FirstOrDefault();
            series = new HighCharts.Series() { name = "High" };
            seriesList.Add(series);
            data = new List<HighCharts.DataPoint>();
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { x = iteration.IterationID, y = injectionRate.NumberOfHighDefects, name = component.ComponentName });
                }
            }
            series.data = data.ToArray();

            // Medium
            series = new HighCharts.Series() { name = ("Medium") };
            seriesList.Add(series);
            data = new List<HighCharts.DataPoint>();
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { x = iteration.IterationID, y = injectionRate.NumberOfMediumDefects, name = component.ComponentName });
                }
            }
            series.data = data.ToArray();

            // Low
            series = new HighCharts.Series() { name = ("Low") };
            seriesList.Add(series);
            data = new List<HighCharts.DataPoint>();
            foreach (var iteration in Iterations)
            {
                foreach (var injectionRate in iteration.DefectInjectionRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { x = iteration.IterationID, y = injectionRate.NumberOfLowDefects, name = component.ComponentName });
                }
            }
            series.data = data.ToArray();

            chart.series = seriesList.ToArray();
            chart.plotOptions = new HighCharts.PlotOptions() { series = new HighCharts.SeriesPlotOptions() { stacking = HighCharts.HighChart.NormalStacking } };

            return chart;
        }
    }
}