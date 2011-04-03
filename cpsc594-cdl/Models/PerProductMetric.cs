using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public abstract class PerProductMetric : Metric
    {
        public abstract string GenerateOverviewGraph(string title, IEnumerable<Product> products);

        public PerProductMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public string GetCacheCode(int[] productIDs)
        {
            Array.Sort(iterationIDs);
            Array.Sort(productIDs);
            return this.ID + "--" + string.Join("-", iterationIDs) + "--" + string.Join("-", productIDs);
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