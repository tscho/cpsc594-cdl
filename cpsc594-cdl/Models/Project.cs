using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public List<Component> Components;

        public Project(int ID, string Name)
        {
            this.ProjectID = ID;
            this.Name = Name;
        }
    }
}