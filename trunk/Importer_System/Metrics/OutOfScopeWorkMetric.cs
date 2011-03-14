using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;

namespace Importer_System.Metrics
{
    class OutOfScopeWorkMetric : Metric
    {

         private Iteration iteration;
        /// <summary>
        ///     Calculates the given log file 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        public void CalculateMetric(string projectDataPath, Iteration curIteration)
        {
            // If we have a directory to check for .xls files
            this.iteration = curIteration;
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+projectDataPath+";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if(xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], [Person Name], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' and [Scope]='False' GROUP BY [Product], [Person Name]");

                    List<string[]> workHours = xlsReader.SelectQuery(query);
                    foreach (string[] row in workHours)
                    {
                        string projectName = row[0];
                        string personName = row[1];
                        double personHours = Double.Parse(row[2]);
                        // Store data
                        if(StoreMetric(projectName, personName, personHours)==-1)
                            Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] Problem storing the resource utilization data to the database, please run the script again and make sure the database schema is correct. " + projectDataPath);
                    }
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] Project data file cannot properly be parsed due to its columns " + projectDataPath);
                }
            } else
                Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] Unable to open project data file " + projectDataPath);
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(string projectName, string personName, double hours)
        {
            return DatabaseAccessor.WriteOutOfScopeWork(projectName, personName, hours, iteration.IterationID);
        }
   }
}
