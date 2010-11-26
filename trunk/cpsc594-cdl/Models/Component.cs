using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Component
    {
        public int ComponentID { get; private set; }
        public int ProjectID { get; private set; }
        public string Name { get; set; }
        private List<IMetric> Metrics;

        public Component(int ID, String Name)
        {
            this.Name = Name;
            this.ID = ID;
        }
    }
}