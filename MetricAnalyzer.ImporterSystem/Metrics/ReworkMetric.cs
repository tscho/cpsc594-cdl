using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.ImporterSystem.Metrics
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
        /// <param name="productDataPath"></param>
        /// <param name="curIteration"></param>
        public void CalculateMetric(string productDataPath, Iteration curIteration)
        {
            // If we have a directory to check for .xls files
            this.iteration = curIteration;
            double sumRework;
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + productDataPath + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    List<Product> productList = DatabaseAccessor.GetProducts();
                    foreach (var currProduct in productList)
                    {
                        string query = String.Concat("Select [Product], [Work Action ID], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                      iteration.IterationLabel, "' AND [Product]='", currProduct.ProductName, "' GROUP BY [Product], [Work Action ID]");
                        List<string[]> workHours = xlsReader.SelectQuery(query);
                        sumRework = 0;
                        if (workHours.Count() != 0)
                        {
                            foreach (string[] row in workHours)
                            {
                                string productName = row[0];
                                int workActionId = Int16.Parse(row[1]);
                                double reworkHours = Double.Parse(row[2]);

                                // Store data
                                if (DetermineIfRework(workActionId, productName))
                                {
                                    sumRework += reworkHours;
                                }
                            }
                            if (StoreMetric(currProduct.ProductName, sumRework) == -1)
                                Reporter.AddErrorMessageToReporter("[Metric 7: Re-work] Problem storing the rework data to the database, please run the script again and make sure the database schema is correct. " + productDataPath);
                        }

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

        public bool DetermineIfRework(int workActionId, string product)
        {
            return DatabaseAccessor.CheckForRework(product, iteration.IterationID, workActionId);
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
