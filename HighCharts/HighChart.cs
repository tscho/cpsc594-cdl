using System;
namespace HighCharts
{
	public class HighChart
	{
        public const string NormalStacking = "normal";
        public const string PercentStacking = "percent";

		public ChartOptions chart;
        public PlotOptions plotOptions = new PlotOptions();
		public TextObject title;
		public XAxisObject xAxis;
		public YAxisObject yAxis;
		public Series[] series;
	}
}

