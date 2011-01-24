using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public interface IMetric
    {
        int ComponentID { get; set; }
        int IterationID { get; set; }
        DateTime TimeStamp { get; set; }
        //IEnumerable<Iteration> Iterations { get; set; }

        int GetValue();
    }
}