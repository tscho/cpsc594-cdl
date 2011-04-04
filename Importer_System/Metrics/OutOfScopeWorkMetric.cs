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
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        public void CalculateMetric(string productDataPath, Iteration curIteration)
        {
            // If we have a directory to check for .xls files
            this.iteration = curIteration;
            double personHours = 0;
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+productDataPath+";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if(xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' and [Scope]='False' GROUP BY [Product]");
                    personHours = 0;
                    List<string[]> workHours = xlsReader.SelectQuery(query);
                    if (workHours.Count() != 0)
                    {
                        foreach (string[] row in workHours)
                        {
                            string productName = row[0];
                            personHours = Double.Parse(row[1]);
                            // Store data
                            if (StoreMetric(productName, personHours) == -1)
                                Reporter.AddErrorMessageToReporter("[Metric 6: Out of Scope Work] Problem storing the out of scope work data to the database." + productDataPath);
                        }
                    }
                    else
                    {
                        query = String.Concat("Select [Product] from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' GROUP BY [Product]");
                        personHours = 0;
                        workHours.Clear();
                        workHours = xlsReader.SelectQuery(query);
                        foreach (string[] row in workHours)
                        {
                            string productName = row[0];
                            if (StoreMetric(productName, personHours) == -1)
                                Reporter.AddErrorMessageToReporter("[Metric 6: Out of Scope Work] Problem storing the out of scope work data to the database." + productDataPath);
                        }
                   }

                    
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("[Metric 6: Out of Scope Work] product data file cannot properly be parsed due to its columns " + productDataPath);
                }
            } else
                Reporter.AddErrorMessageToReporter("[Metric 6: Out of Scope Work] Unable to open product data file " + productDataPath);
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(string productName, double hours)
        {
            return DatabaseAccessor.WriteOutOfScopeWork(productName, hours, iteration.IterationID);
        }
   }
}
