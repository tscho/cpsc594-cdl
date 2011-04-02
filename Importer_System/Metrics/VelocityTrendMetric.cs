using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;

namespace Importer_System.Metrics
{
    class VelocityTrendMetric : Metric
    {
        private Iteration iteration;
        /// <summary>
        ///     Calculates the given log file 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        public void CalculateMetric(string productDataPath, Iteration currIteration)
        {
            // If we have a directory to check for .xls files
            this.iteration = currIteration;
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + productDataPath + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], Sum([Estimate]), Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' and [Scope]='True' GROUP BY [Product]");

                    List<string[]> workHours = xlsReader.SelectQuery(query);
                    foreach (string[] row in workHours)
                    {
                        string productName = row[0];
                        int contractID = Int32.Parse(row[1]);
                        double estimatedHours = Double.Parse(row[2]);
                        double actualHours = Double.Parse(row[3]);
                        // Store data
                        if (StoreMetric(productName, estimatedHours, actualHours) == -1)
                            Reporter.AddErrorMessageToReporter("[Metric 8: Velocity Trend] Problem storing the resource utilization data to the database, please run the script again and make sure the database schema is correct. " + productDataPath);
                    }
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("[Metric 8: Velocity Trend] product data file cannot properly be parsed due to its columns " + productDataPath);
                }
            }
            else
                Reporter.AddErrorMessageToReporter("[Metric 8: Velocity Trend] Unable to open product data file " + productDataPath);
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(string productName, double estimatedHours, double actualHours)
        {
            return DatabaseAccessor.WriteVelocityTrendMetric(productName, estimatedHours, actualHours, iteration.IterationID);
        }
    }
}
