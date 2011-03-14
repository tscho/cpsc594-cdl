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
    public class OutOfScopeWorkMetric : Metric
    {
        //
        // GET: /OutOfScopeWorkMetric/
        public override string Name { get { return "Out of Scope Work"; } }
        public override int ID { get { return (int)MetricType.OutOfScopeWork; } }
        public override bool OverviewOnly { get { return true; } }

        public OutOfScopeWorkMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, IEnumerable<Component> components)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Person";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Out Of Scope Work(hours)";

            IEnumerable<int> componentIds = components.Select(x => x.ComponentID);
            Series series;
            foreach (var iteration in Iterations)
            {

            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(componentIds.ToArray()), chart);
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            return GenerateOverviewGraph(title, new Component[] { component });
        }

    }
}
