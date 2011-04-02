using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;

namespace Importer_System.Metrics
{
    class ReworkMetric : Metric
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
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + productDataPath + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' and [Scope]='False' GROUP BY [Product]");

                    List<string[]> workHours = xlsReader.SelectQuery(query);
                    foreach (string[] row in workHours)
                    {
                        string productName = row[0];
                        int contractID = Int32.Parse(row[1]);
                        double reworkHours = Double.Parse(row[2]);
                        // Store data
                        if (StoreMetric(productName, reworkHours) == -1)
                            Reporter.AddErrorMessageToReporter("[Metric 7: Re-work] Problem storing the resource utilization data to the database, please run the script again and make sure the database schema is correct. " + productDataPath);
                    }
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("[Metric 7: Re-work] product data file cannot properly be parsed due to its columns " + productDataPath);
                }
            }
            else
                Reporter.AddErrorMessageToReporter("[Metric 7: Re-work] Unable to open product data file " + productDataPath);
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(string productName, double reworkHours)
        {
            return DatabaseAccessor.WriteReworkMetric(productName, reworkHours, iteration.IterationID);
        }
    }
}
