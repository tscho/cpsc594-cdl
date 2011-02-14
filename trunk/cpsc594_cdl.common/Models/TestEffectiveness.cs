using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsc594_cdl.Common.Models
{
    public partial class TestEffectiveness
    {
        public int getValue()
        {
            DefectInjectionRate inject = this.Iteration.DefectInjectionRates.Last();
            return this.TestCases / inject.GetValue();
        }
    }
}
