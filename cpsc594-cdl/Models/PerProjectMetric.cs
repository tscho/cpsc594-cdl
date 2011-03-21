using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public abstract class PerProductMetric : Metric
    {
        public abstract string GenerateOverviewGraph(string title, Product product);

        public PerProductMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public string GetCacheCode(int projectID)
        {
            Array.Sort(iterationIDs);
            return this.ID + "--" + string.Join("-", iterationIDs) + "--" + projectID;
        }
    }
}