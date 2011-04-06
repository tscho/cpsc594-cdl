using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricAnalyzer.Common.Models
{
    public partial class Coverage
    {
        public double GetCoverage()
        {
            return (1.0 * LinesCovered / (LinesExecuted > 0 ? LinesExecuted : 1)) * 100; //can't divide by zero! Although lc shouldn't really ever be 0
        }
    }
}