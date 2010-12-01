﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cpsc594_cdl.Models
{
    public class IndexModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Component> Components { get; set; }
        public IEnumerable<Iteration> Iterations { get; set; }

        public string ProjectID { get; set; }
        public string StartYear { get; set; }
        public string StartMonth { get; set; }
        public string StartDay { get; set; }
        public IEnumerable<int> ComponentIDs { get; set; }
        public IEnumerable<int> MetricIDs { get; set; }
    }
}
