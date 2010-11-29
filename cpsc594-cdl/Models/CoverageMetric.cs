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


        public int ComponentID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int IterationID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime TimeStamp
        {
            get { return TimeStamp; }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<Iteration> Iterations
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public CoverageMetric(int coverageID, int iterationID, int linesExecuted,int linesCovered, DateTime iterationDate)
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
    }

    
}