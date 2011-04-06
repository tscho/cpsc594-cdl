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
    public class DefectRepairRateMetric : PerComponentMetric
    {
        public override string Name { get { return "Defect Repair Rate"; }}
        public override int ID { get { return (int)MetricType.DefectRepairRate;  } }

        public DefectRepairRateMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        // x: components, y: defects
        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Components";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Total Number of Defects\n(Resolved, Verified Defects)";

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.DefectRepairRates == null || iteration.DefectRepairRates.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var repairRate in iteration.DefectRepairRates.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    int value = (int)(repairRate.NumberOfResolvedDefects + repairRate.NumberOfVerifiedDefects);
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = repairRate.Component.ComponentName;
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(componentIds.ToArray()), chart);
        }

        // x: iterations, y: defects
        public override string GenerateComponentGraph(string title, Component component)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Iterations";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Number of Defects";

            Series series;
            double value;
            // Resolved
            series = new Series("Resolved");
            chart.Series.Add(series);
            foreach (var iteration in Iterations)
            {
                foreach (var repairRate in iteration.DefectRepairRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    value = (int)repairRate.NumberOfResolvedDefects;
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate.ToShortDateString();
                }
            }
            // Verified
            series = new Series("Verified");
            chart.Series.Add(series);
            foreach (var iteration in Iterations)
            {
                foreach (var repairRate in iteration.DefectRepairRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    value = (int)repairRate.NumberOfVerifiedDefects;
                    series.Points.AddY(value);
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = iteration.StartDate.ToShortDateString();
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(component.ComponentID), chart);
        }
    }
}