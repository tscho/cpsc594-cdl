using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Importer_System
{
    public abstract class Metric
    {
        /// <summary>
        ///     Calculates metric.
        /// </summary>
        void CalculateMetric(){}
        /// <summary>
        ///     Method call to database for query to store metric data.
        /// </summary>
        void StoreMetric(){}
    }
}
