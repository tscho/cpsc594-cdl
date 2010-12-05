using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Iteration
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int iterationID { get; set; }
        public CoverageMetric coverage { get; set; }

        public Iteration(DateTime start, DateTime end, int iterationID, CoverageMetric coverage)
        {
            this.StartDate = start;
            this.EndDate = end;
            this.coverage = coverage;
            this.iterationID = iterationID; 
        }

        public Iteration clone()
        {
            CoverageMetric cm = null;
            if (coverage!=null)
                cm = new CoverageMetric(coverage.ComponentID, coverage.IterationID, coverage.GetValue(), coverage.GetLinesCovered(), coverage.TimeStamp);
            Iteration result = new Iteration(StartDate, EndDate, iterationID, cm);
            return result;
        }

    }
}