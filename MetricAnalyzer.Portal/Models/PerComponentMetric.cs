using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Models
{
    public abstract class PerComponentMetric : Metric
    {
        public abstract string GenerateOverviewGraph(string title, IEnumerable<Component> components);
        public abstract string GenerateComponentGraph(string title, Component component);
        public abstract HighCharts.HighChart GenerateHighChart(string title, string target, IEnumerable<Component> components);
        public abstract HighCharts.HighChart GenerateOverviewHighChart(string title, string target, IEnumerable<Component> components);

        public PerComponentMetric(IEnumerable<Iteration> iterations) : base(iterations) { }

        public string StringEncode(int[] componentIDs)
        {
            Array.Sort(iterationIDs);
            Array.Sort(componentIDs);
            return this.ID + "--" + string.Join("-", iterationIDs) + "--" + string.Join("-", componentIDs);
        }

        public string StringEncode(int componentID)
        {
            return StringEncode(new int[] { componentID });
        }

        public static IEnumerable<int> IDs
        {
            get
            {
                foreach (MetricType t in new MetricType[] { MetricType.Coverage, MetricType.DefectInjectionRate, 
                MetricType.DefectRepairRate })
                {
                    yield return (int)t;
                }
            }
        }
    }
}