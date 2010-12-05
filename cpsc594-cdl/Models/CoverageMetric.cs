using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class CoverageMetric : IMetric
    {
        int linesExecuted;
        int linesCovered;

        public int ComponentID { get; set; }
        public int IterationID { get; set; }
        public DateTime TimeStamp { get; set; }
        public IEnumerable<Iteration> Iterations { get; set; }

        public CoverageMetric(int coverageID, int iterationID, int linesExecuted, int linesCovered, DateTime iterationDate)
        {
            this.ComponentID = coverageID;
            this.IterationID = iterationID;
            this.linesExecuted = linesExecuted;
            this.linesCovered = linesCovered;
            this.TimeStamp = iterationDate;
        }

        public int GetValue()
        {
            return linesExecuted;
        }

        public int GetLinesCovered()
        {
            return linesCovered;
        }
    }

    
}