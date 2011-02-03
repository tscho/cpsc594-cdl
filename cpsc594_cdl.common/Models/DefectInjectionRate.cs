using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Common.Models
{
    public partial class DefectInjectionRate
    {
        public int GetValue()
        {
            return NumberOfHighDefects+NumberOfMediumDefects+NumberOfLowDefects ?? 0;
        }

        public int GetHighDefects()
        {
            return NumberOfHighDefects ?? 0;
        }

        public int GetMediumDefects()
        {
            return NumberOfMediumDefects ?? 0;
        }

        public int GetLowDefects()
        {
            return NumberOfLowDefects ?? 0;
        }

    }
}