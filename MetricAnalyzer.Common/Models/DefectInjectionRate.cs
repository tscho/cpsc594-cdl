using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetricAnalyzer.Common.Models
{
    public partial class DefectInjectionRate
    {
        public int GetValue()
        {
            return NumberOfHighDefects+NumberOfMediumDefects+NumberOfLowDefects ;
        }

        public int GetHighDefects()
        {
            return NumberOfHighDefects ;
        }

        public int GetMediumDefects()
        {
            return NumberOfMediumDefects;
        }

        public int GetLowDefects()
        {
            return NumberOfLowDefects;
        }

    }
}