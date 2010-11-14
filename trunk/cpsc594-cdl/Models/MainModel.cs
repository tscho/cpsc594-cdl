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
        public string ProjectID { get; set; }
        [Required]
        public IEnumerable<int> Components { get; set; }
        [Required]
        public IEnumerable<int> Metrics { get; set; }
    }

    public class ComponentModel
    {
        [Required]
        public Object Chart1 { get; set; }
    }

    public class StaticModel
    {
        public static List<int> createStaticData()
        {
            List<int> c_data = new List<int>();
            c_data.Add(1);
            c_data.Add(1);
            c_data.Add(6);
            c_data.Add(4);
            c_data.Add(3);
            return c_data;
        }
    }
}
