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
            return 0 ?? NumberOfHighDefects+NumberOfMediumDefects+NumberOfLowDefects ;
        }

        public int GetHighDefects()
        {
            return 0 ?? NumberOfHighDefects ;
        }

        public int GetMediumDefects()
        {
            return 0 ?? NumberOfMediumDefects;
        }

        public int GetLowDefects()
        {
            return 0 ?? NumberOfLowDefects;
        }

    }
}