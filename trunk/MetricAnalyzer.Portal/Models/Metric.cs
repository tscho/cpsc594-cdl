using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetricAnalyzer.Common.Models;
using System.Reflection;

namespace MetricAnalyzer.Portal.Models
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

        public static Metric CreateMetricInstanceFromEncodedString(string encodedString)
        {
            var parts = getEncodedParts(encodedString);

            MetricType metricType = (MetricType)int.Parse(parts[0]);

            var iterationIDs = getIDsFromString(parts[1]);

            return CreateMetricInstanceFromType(metricType, DatabaseAccessor.GetIterations(iterationIDs));
        }

        public static IEnumerable<Component> GetComponentsFromEncodedString(string encodedString)
        {
            var componentIDs = getIDsFromString(getEncodedParts(encodedString)[2]);

            return DatabaseAccessor.GetComponents(componentIDs);
        }

        public static IEnumerable<Product> GetProductsFromEncodedString(string encodedString)
        {
            var productIDs = getIDsFromString(getEncodedParts(encodedString)[2]);

            return DatabaseAccessor.GetProducts(productIDs);
        }

        private static string[] getEncodedParts(string encodedString)
        {
            return System.Text.RegularExpressions.Regex.Split(encodedString, "--");
        }

        private static IEnumerable<int> getIDsFromString(string encodedString)
        {
            return encodedString.Split('-').Select<string, int>(x => int.Parse(x));
        }


    }
}