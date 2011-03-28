using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsc594_cdl.Common.Models
{
    partial class VelocityTrend
    {
        public int getValue()
        {
            return Convert.ToInt32(this.EstimatedHours/this.ActualHours);
        }
    }
}
