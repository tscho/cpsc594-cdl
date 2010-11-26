using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class CoverageMetric : IMetric
    {
        IEnumerable<Iteration> Iterations { get; set; }
    }
}