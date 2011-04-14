using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetricAnalyzer.Portal.Models;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Models
{
    public class IndexModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Component> Components { get; set; }
        public IEnumerable<Iteration> Iterations { get; set; }

        public Product Product { get; set; }
        //public string ProductName { get; set; }

        public int[] MetricIDs { get; set; }

        public int? ProductID { get; set; }
        public IEnumerable<int> ComponentIDs { get; set; }

        public IEnumerable<int> ProductIDs { get; set; }
        public int StartIteration { get; set; }
        public int EndIteration { get; set; }

        public List<PerComponentMetric> ComponentMetrics { get; set; }
        public List<PerProductMetric> ProductMetrics { get; set; }

        public IEnumerable<Object> Metrics
        {
            get
            {
                List<Object> metrics = new List<object>();
                foreach (var metric in Enum.GetNames(typeof(MetricType)))
                    metrics.Add(new { Name = metric, Id = (int)Enum.Parse(typeof(MetricType), metric) });
                return metrics;
            }
        }
    }
}