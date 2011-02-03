using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Models
{
    public interface IMetric
    {
        string Name { get; }
        string GenerateGraph(string title, IEnumerable<Component> components);
        string GenerateGraph(string title, Component component);
    }
}