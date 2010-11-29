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
        public List<Iteration> Iterations;

        public Component(int componentID, int projectID, String Name)
        {
            this.Name = Name;
            this.ComponentID = componentID;
            this.ProjectID = projectID;
        }
    }
}