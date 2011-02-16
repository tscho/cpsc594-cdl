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
            DefectInjectionRate inject = this.Iteration.DefectInjectionRates.FirstOrDefault(x => x.ComponentID == this.ComponentID);
            return this.TestCases / (inject.GetValue() == 0 ? 1 : inject.GetValue());
        }
    }
}
