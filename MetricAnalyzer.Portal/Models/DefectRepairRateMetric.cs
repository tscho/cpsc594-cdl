﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public override HighCharts.HighChart GenerateOverviewHighChart(string title, string target, IEnumerable<Component> components)
        {
            var chart = new HighCharts.HighChart();

            chart.chart = new HighCharts.ChartOptions() { renderTo = target };
            chart.title = new HighCharts.TextObject(title);
            chart.xAxis = new HighCharts.XAxisObject() { categories = new string[components.Count()], title = new HighCharts.TextObject("Components") };
            chart.yAxis = new HighCharts.YAxisObject() { title = new HighCharts.TextObject("Total Number of Defects\n(Resolved, Verified Defects)") };

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
                if (iteration.DefectRepairRates == null || iteration.DefectRepairRates.Count == 0)
                    continue;

                series = new HighCharts.Series() { name = iteration.IterationLabel };
                data = new List<HighCharts.DataPoint>();

                foreach(var repairRate in iteration.DefectRepairRates.Where(x => componentIds.Contains(x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { name = repairRate.Component.ComponentName, x = xTranslate[repairRate.ComponentID], y = repairRate.GetValue() });
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

            // Resolved
            var component = components.FirstOrDefault();
            series = new HighCharts.Series() { name = "Resolved" };
            seriesList.Add(series);
            data = new List<HighCharts.DataPoint>();
            foreach (var iteration in Iterations)
            {
                foreach (var repairRate in iteration.DefectRepairRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { x = iteration.IterationID, y = repairRate.NumberOfResolvedDefects, name = component.ComponentName });
                }
            }
            series.data = data.ToArray();

            // Verified
            series = new HighCharts.Series() { name = ("Verified") };
            seriesList.Add(series);
            data = new List<HighCharts.DataPoint>();
            foreach (var iteration in Iterations)
            {
                foreach (var repairRate in iteration.DefectRepairRates.Where(x => (component.ComponentID == x.ComponentID)))
                {
                    data.Add(new HighCharts.DataPoint() { x = iteration.IterationID, y = repairRate.NumberOfVerifiedDefects, name = component.ComponentName });
                }
            }
            series.data = data.ToArray();

            chart.series = seriesList.ToArray();
            chart.plotOptions = new HighCharts.PlotOptions() { series = new HighCharts.SeriesPlotOptions() { stacking = HighCharts.HighChart.NormalStacking } };

            return chart;
        }
    }
}