﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Util.Database;

namespace cpsc594_cdl.Models.Repository
{
    public class MetricRepository
    {

        public MetricRepository() {
            
        }

        public CoverageMetric getCoverage(int iterationID, int componentID, String iterationDate)
        {
            Util.Database.Coverage dbCoverage = DatabaseAccessor.GetCoverage(iterationID, componentID);
            var coverage = new CoverageMetric(dbCoverage.ComponentID, dbCoverage.IterationID, dbCoverage.LinesExecuted,
                                             dbCoverage.LinesCovered, Convert.ToDateTime(iterationDate));
            return coverage;
        }
    }
}