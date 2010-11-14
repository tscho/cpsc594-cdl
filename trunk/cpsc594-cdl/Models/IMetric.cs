using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public interface IMetric
    {
        DateTime TimeStamp { get; set; }
        int GetValue();
    }
}