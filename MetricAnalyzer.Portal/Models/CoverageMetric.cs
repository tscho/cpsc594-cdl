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
    public class CoverageMetric : PerComponentMetric
    {
        public override string Name { get { return "Code Coverage"; }}
        public override int ID { get { return (int)MetricType.Coverage;  } }

        public CoverageMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

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