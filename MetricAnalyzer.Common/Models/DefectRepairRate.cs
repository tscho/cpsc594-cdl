using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricAnalyzer.Common.Models
{
    public partial class DefectRepairRate
    {
        public double GetValue()
        {
            return this.NumberOfResolvedDefects + this.NumberOfVerifiedDefects;
        }
    }
}
