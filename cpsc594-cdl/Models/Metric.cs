using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;
using System.Reflection;

namespace cpsc594_cdl.Models
{
    public abstract class Metric
    {
        protected Iteration[] Iterations;
        protected int[] iterationIDs;

        public Metric(IEnumerable<Iteration> iterations)
        {
            Iterations = iterations.ToArray();
            iterationIDs = Iterations.Select(x => x.IterationID).ToArray();
        }

        public abstract string Name { get; }
        public abstract int ID { get; }

        public static Metric CreateMetricInstanceFromType(MetricType metricType, IEnumerable<Iteration> constructorArg)
        {
            string metricName = typeof(Metric).Namespace + "." + metricType.ToString() + "Metric";
            Type t = Type.GetType(metricName, true);

            ConstructorInfo c = t.GetConstructor(new Type[] { typeof(IEnumerable<Iteration>) });

            Metric m = (Metric)c.Invoke(new object[] { constructorArg });

            return m;
        }

    }
}