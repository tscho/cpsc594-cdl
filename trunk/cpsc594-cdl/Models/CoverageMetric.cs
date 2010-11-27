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

        public DateTime TimeStamp
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GetValue()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Iteration> Iterations
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


    }

    
}