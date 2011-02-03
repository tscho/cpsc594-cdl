using System;
using System.Collections.Generic;
using System.Linq;

namespace cpsc594_cdl.Common.Models
{
    public partial class Coverage
    {
        public double GetCoverage()
        {
            return (1.0 * LinesExecuted / (LinesCovered > 0 ? LinesCovered : 1)) * 100; //can't divide by zero! Although lc shouldn't really ever be 0
        }
    }
}