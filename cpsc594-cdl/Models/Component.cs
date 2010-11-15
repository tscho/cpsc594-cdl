using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Component
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        private List<IMetric> Metrics;

        public Component(int ID, String Name)
        {
            this.Name = Name;
            this.ID = ID;
        }
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