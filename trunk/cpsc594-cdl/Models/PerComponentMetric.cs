﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public abstract class PerComponentMetric : Metric
    {
        public abstract string GenerateOverviewGraph(string title, IEnumerable<Component> components);
        public abstract string GenerateComponentGraph(string title, Component component);

        public PerComponentMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public string GetCacheCode(int[] componentIDs)
        {
            Array.Sort(iterationIDs);
            Array.Sort(componentIDs);
            return this.ID + "--" + string.Join("-", iterationIDs) + "--" + string.Join("-", componentIDs);
        }

        public string GetCacheCode(int componentID)
        {
            return GetCacheCode(new int[] { componentID });
        }
    }
}