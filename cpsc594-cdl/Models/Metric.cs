using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public abstract class Metric
    {
        protected Iteration[] Iterations;

        public Metric(IEnumerable<Iteration> iterations)
        {
            Iterations = iterations.ToArray();
        }

        public abstract string Name { get; }
        public abstract int ID { get; }
        public abstract string GenerateOverviewGraph(string title, IEnumerable<Component> components);
        public abstract string GenerateComponentGraph(string title, Component component);
    }
}