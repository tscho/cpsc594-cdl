using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace cpsc594_cdl.Infrastructure
{
    public class ChartFactory
    {
        public static Chart CreateChart(string title)
        {
            Chart chart = new Chart();
            chart.Width = 1024;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title chartTitle = new Title(title, Docking.Top, new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(chartTitle);
            chart.Legends.Add("Legend");
            chart.ChartAreas.Add("ChartArea");
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Maximum = 100;

            return chart;
        }
    }
}