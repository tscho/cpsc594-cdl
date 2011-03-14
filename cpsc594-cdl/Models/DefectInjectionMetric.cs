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
    public class DefectInjectionMetric : Metric
    {
        public override string Name { get { return "Defect Injection Rate";  } }
        public override int ID { get { return (int)MetricType.DefectInjectionRate;  } }

        public DefectInjectionMetric(IEnumerable<Iteration> iterations) : base(iterations) {}

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

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(componentIds.ToArray()), chart);
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

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(component.ComponentID), chart);
        }
    }
}