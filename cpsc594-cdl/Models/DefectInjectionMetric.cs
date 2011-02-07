﻿using System;
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

            Series series;
            foreach (var component in components)
            {
                foreach (var iteration in Iterations)
                {
                    if ((series = chart.Series.FindByName(iteration.StartDate.ToShortDateString())) == null)
                    {
                        series = new Series(iteration.StartDate.ToShortDateString());
                        chart.Series.Add(series);
                    }
                    series.Points.AddY(iteration.DefectInjectionRates.Single(x => x.ComponentID == component.ComponentID).GetValue());
                    series.Points.Last().MarkerSize = 10;
                    series.Points.Last().AxisLabel = component.ComponentName;
                }
            }

            MemoryStream imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;

            string base64_output = System.Convert.ToBase64String(imageStream.ToArray());
            return base64_output;
        }

        public override string GenerateComponentGraph(string title, Component component)
        {
            return GenerateOverviewGraph(title, new Component[] { component });
        }
    }
}