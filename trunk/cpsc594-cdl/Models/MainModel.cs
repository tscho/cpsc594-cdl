using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public class IndexModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Component> Components { get; set; }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<int> ComponentIDs { get; set; }
        public IEnumerable<int> MetricIDs { get; set; }

        public List<Metric> Metrics { get; set; }
    }
}