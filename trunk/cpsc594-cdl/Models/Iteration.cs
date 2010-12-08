using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Iteration
    {
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public int iterationID { get; set; }
        public CoverageMetric coverage { get; set; }

        public Iteration(DateTime start, DateTime end, int iterationID, CoverageMetric coverage)
        {
            this.StartDate = start.ToString("dd/MM/yyyy");
            this.EndDate = end.ToString("dd/MM/yyyy");
            this.coverage = coverage;
            this.iterationID = iterationID; 
        }

        public Iteration clone()
        {
            CoverageMetric cm = null;
            if (coverage!=null)
                cm = new CoverageMetric(coverage.ComponentID, coverage.IterationID, coverage.GetValue(), coverage.GetLinesCovered(), coverage.TimeStamp);
            Iteration result = new Iteration(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), iterationID, cm);
            return result;
        }

    }
}