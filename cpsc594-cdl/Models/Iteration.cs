using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Iteration
    {
        DateTime StartDate { get; private set; }
        DateTime EndDate { get; private set; }
        CoverageMetric coverage { get; private set; }

        public Iteration(DateTime start, DateTime end, CoverageMetric coverage)
        {
            this.StartDate = start;
            this.EndDate = end;
            this.coverage = coverage;
        }
    }
}