using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public class IndexModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Component> Components { get; set; }
        public IEnumerable<Iteration> Iterations { get; set; }

        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string StartDate { get; set; }
        public IEnumerable<int> ComponentIDs { get; set; }
        public IEnumerable<int> MetricIDs { get; set; }

        public string Chart1_Base64 { get; set; }
        public string Chart2_Base64 { get; set; }
        public string Chart3_Base64 { get; set; }
        public string Chart4_Base64 { get; set; }
    }
}
