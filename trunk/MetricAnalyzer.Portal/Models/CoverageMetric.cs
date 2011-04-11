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
    public class CoverageMetric : PerComponentMetric
    {
        public override string Name { get { return "Code Coverage"; }}
        public override int ID { get { return (int)MetricType.Coverage;  } }

        public CoverageMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title, true);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Components";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Coverage %";
            
            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            List<int> usedComponentIds = new List<int>();
            
            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.Coverages == null || iteration.Coverages.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);
                foreach(var coverage in iteration.Coverages.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    // Remove Duplicate Component in the same metric first
                    if (!usedComponentIds.Contains(coverage.ComponentID))
                    {
                        usedComponentIds.Add(coverage.ComponentID);

                        series.Points.AddY(coverage.GetCoverage());
                        series.Points.Last().MarkerSize = 10;
                        series.Points.Last().AxisLabel = coverage.Component.ComponentName;
                    }
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.StringEncode(componentIds.ToArray()), chart);
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            return GenerateOverviewGraph(title, new Component[] { component });
        }

        public override HighCharts.HighChart GenerateOverviewHighChart(string title, string target, IEnumerable<Component> components)
        {
            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[components.Count()], title = new HighCharts.TextObject("Components") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Coverage %") };

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
                if (iteration.Coverages == null || iteration.Coverages.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = iteration.IterationLabel };
                data = new List<HighCharts.DataPoint>();

                foreach(var coverage in iteration.Coverages.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { name = coverage.Component.ComponentName, x = xTranslate[coverage.ComponentID], y = coverage.GetCoverage() });
                }

                series.data = data.ToArray();
                seriesList.Add(series);
            }

            chart.series = seriesList.ToArray();
            return chart;
        }

        public override HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Component> components)
        {
            return GenerateOverviewHighChart(title, target, components);
        }
    }
}