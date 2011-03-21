using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cpsc594_cdl.Models;
using cpsc594_cdl.Common.Models;

namespace Portal_Tests
{
    [TestClass]
    public class MetricTests
    {
        [TestMethod]
        public void TestCreateMetricInstanceFromType()
        {
            Metric test;
            IEnumerable<Iteration> iters = new Iteration[] { new Iteration() };

            test = Metric.CreateMetricInstanceFromType(MetricType.Coverage, iters);
            Assert.IsTrue(typeof(CoverageMetric).IsInstanceOfType(test));

            test = Metric.CreateMetricInstanceFromType(MetricType.DefectInjectionRate, iters);
            Assert.IsTrue(typeof(DefectInjectionRateMetric).IsInstanceOfType(test));

            test = Metric.CreateMetricInstanceFromType(MetricType.DefectRepairRate, iters);
            Assert.IsTrue(typeof(DefectRepairRateMetric).IsInstanceOfType(test));

            test = Metric.CreateMetricInstanceFromType(MetricType.OutOfScopeWork, iters);
            Assert.IsTrue(typeof(OutOfScopeWorkMetric).IsInstanceOfType(test));

            test = Metric.CreateMetricInstanceFromType(MetricType.ResourceUtilization, iters);
            Assert.IsTrue(typeof(ResourceUtilizationMetric).IsInstanceOfType(test));

            test = Metric.CreateMetricInstanceFromType(MetricType.TestEffectiveness, iters);
            Assert.IsTrue(typeof(TestEffectivenessMetric).IsInstanceOfType(test));
        }
    }
}
