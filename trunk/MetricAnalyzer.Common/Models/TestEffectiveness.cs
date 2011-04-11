using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricAnalyzer.Common.Models
{
    public partial class TestEffectiveness
    {
        public float getValue()
        {
            float totalDefects = this.Product.Components.Aggregate<Component, float>(0, 
                (x, comp) => x + comp.DefectInjectionRates.Aggregate<DefectInjectionRate, int>(0, 
                    (y, injRate) => y + injRate.GetValue()));
            return totalDefects / (this.TestCases > 0 ? this.TestCases : 1);
        }
    }
}