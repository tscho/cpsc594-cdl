using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetricAnalyzer.Portal.Models;
using MetricAnalyzer.Common.Models;

namespace Portal_Tests.Models
{
    [TestClass]
    public class PerComponentMetricTests
    {
        [TestMethod]
        public void SimpleCacheIDTest()
        {
            int[] componentIds = new int[] { 1, 2, 3 };
            List<Iteration> iterations = new List<Iteration>();
            iterations.Add(new Iteration { IterationID = 8 });
            iterations.Add(new Iteration { IterationID = 33 });
            iterations.Add(new Iteration { IterationID = 6 });
            iterations.Add(new Iteration { IterationID = 2 });

            CoverageMetric cov1 = new CoverageMetric(iterations);
            CoverageMetric cov2 = new CoverageMetric(iterations);

            var cache1 = cov1.StringEncode(componentIds);
            var cache2 = cov2.StringEncode(componentIds);

            Assert.AreEqual(cache1, cache2);
        }

        [TestMethod]
        public void ComplexCacheIDTest()
        {
            CoverageMetric cov1, cov2, cov3, cov4;
            List<Iteration> iterations1, iterations2, iterations3, iterations4;
            List<int> componentIds1, componentIds2, componentIds3;

            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                iterations1 = new List<Iteration>();
                iterations2 = new List<Iteration>();
                iterations3 = new List<Iteration>();
                iterations4 = new List<Iteration>();
                for (int j = 0; j < rand.Next(100); j++)
                {
                    var iterId = rand.Next(1000);
                    iterations1.Add(new Iteration() { IterationID = iterId });
                    iterations2.Add(new Iteration() { IterationID = iterId });
                    iterations3.Add(new Iteration() { IterationID = iterId });
                    iterations4.Add(new Iteration() { IterationID = iterId });
                }

                cov1 = new CoverageMetric(iterations1);
                cov2 = new CoverageMetric(iterations2);
                cov3 = new CoverageMetric(iterations3);
                cov4 = new CoverageMetric(iterations4);

                componentIds1 = new List<int>();
                componentIds2 = new List<int>();
                componentIds3 = new List<int>();
                for (int j = 0; j < rand.Next(100); j++)
                {
                    var id = rand.Next(1000);
                    componentIds1.Add(id);
                    componentIds2.Add(id);
                    componentIds3.Add(id);
                }

                var cache1 = cov1.StringEncode(componentIds1.ToArray());
                var cache2 = cov2.StringEncode(componentIds2.ToArray());
                Assert.AreEqual(cache1, cache2);

                componentIds3.Reverse();
                var cache3 = cov3.StringEncode(componentIds3.ToArray());
                Assert.AreEqual(cache1, cache3);

                var cache4 = cov4.StringEncode(genRandomIntArray(102, 1000, true));
                Assert.AreNotEqual(cache1, cache4);
            }
        }

        private int[] genRandomIntArray(int maxLen, int maxVal, bool max = false)
        {
            List<int> generator = new List<int>();
            Random rand = new Random();

            for (int i = 0; i < (max ? maxLen : rand.Next(maxLen)); i++)
            {
                generator.Add(rand.Next(1, maxVal));
            }

            return generator.ToArray();
        }
    }
}
