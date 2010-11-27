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
    }
}