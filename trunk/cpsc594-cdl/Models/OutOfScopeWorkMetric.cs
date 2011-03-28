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
    public class OutOfScopeWorkMetric : PerProductMetric
    {
        public override string Name { get { return "Out of Scope Work"; } }
        public override int ID { get { return (int)MetricType.OutOfScopeWork; } }

        public OutOfScopeWorkMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public override string GenerateOverviewGraph(string title, Product product)
        {
            Chart chart = ChartFactory.CreateChart(title);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.Title = "Contract ID";
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.Title = "Out Of Scope Work(hours)";

            Series series;
            foreach (var iteration in Iterations)
            {
                if (iteration.OutOfScopeWorks == null || iteration.OutOfScopeWorks.Count == 0)
                    continue;

                series = new Series(iteration.StartDate.ToShortDateString());
                chart.Series.Add(series);

                //OutOfScopeWork hours = iteration.OutOfScopeWorks.Where(x => x.ProductID == product.ProductID).Aggregate((x, next) => { x.PersonHours += next.PersonHours; return x; });

                //series.Points.AddY(hours.PersonHours);

                foreach (var oos in iteration.OutOfScopeWorks.Where(x => x.ProductID == product.ProductID))
                {
                    //var existingPoints = series.Points.Where(x => x.XValue == oos.ContractID.GetHashCode());
                    var existingPoints = series.Points.Where(x => x.XValue == oos.ContractID);
                    if (existingPoints.Count() != 0)
                    {
                        existingPoints.First().YValues[0] += oos.PersonHours;
                    }
                    else
                    {
                        series.Points.AddXY(oos.ContractID, oos.PersonHours);
                        series.Points.Last().MarkerSize = 10;
                    }
                }
            }

            return ChartImageCache.GetImageCache().SaveChartImage(this.GetCacheCode(product.ProductID), chart);
        }
    }
}
