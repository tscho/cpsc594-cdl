using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Models
{
    public abstract class PerProductMetric : Metric
    {
        public abstract HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Product> products);

        public PerProductMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public string StringEncode(int[] productIDs)
        {
            Array.Sort(iterationIDs);
            Array.Sort(productIDs);
            return this.ID + "--" + string.Join("-", iterationIDs.Select(x=>x.ToString()).ToArray()) + "--" + string.Join("-", productIDs.Select(x=>x.ToString()).ToArray());
        }

        public static IEnumerable<int> IDs
        {
            get
            {
                foreach (MetricType t in new MetricType[] { MetricType.OutOfScopeWork, MetricType.ResourceUtilization,
                MetricType.Rework, MetricType.TestEffectiveness, MetricType.VelocityTrend })
                {
                    yield return (int)t;
                }
            }
        }
    }
}