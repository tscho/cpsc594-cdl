using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cpsc594_cdl.Common.Models
{
    public partial class TestEffectiveness
    {
        //public DefectInjectionRate AssociatedDefectRate { get { return this.Iteration.DefectInjectionRates.FirstOrDefault(x => x.ComponentID == this.ComponentID); } }

        public int getValue()
        {
            //return AssociatedDefectRate.GetValue() / (this.TestCases > 0 ? this.TestCases : 1);
            return 1;
        }
    }
}