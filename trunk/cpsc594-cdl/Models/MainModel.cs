using System;
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
        [Required]
        public string PID { get; set; }
        [Required]
        public string IsSelectProject { get; set; }


        [Required]
        public IEnumerable<Project> Projects { get; set; }
        [Required]
        public string ProjectID { get; set; }
        [Required]
        public IEnumerable<Component> Components { get; set; }
        [Required]
        public IEnumerable<int> ComponentIDs { get; set; }
        [Required]
        public IEnumerable<int> Metrics { get; set; }
    }

    public class ComponentModel
    {
    }
}
