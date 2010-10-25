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
        public string ProjectID { get; set; }
        [Required]
        public string IsSelectProject { get; set; }

        [Required]
        public string CM_Array { get; set; }
    }
}
