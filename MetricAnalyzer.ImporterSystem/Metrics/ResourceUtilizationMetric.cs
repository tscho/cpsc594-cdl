﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetricAnalyzer.Common.Models;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Windows.Forms;

namespace MetricAnalyzer.ImporterSystem.Metrics
{
    class ResourceUtilizationMetric
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
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+productDataPath+";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if(xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], [Work Action ID], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iteration.IterationLabel, "' GROUP BY [Product], [Work Action ID]");
                    List<string[]> workHours = xlsReader.SelectQuery(query);
                    foreach (string[] row in workHours)
                    {
                        string productName = row[0];
                        int workActionId = Int16.Parse(row[1]);
                        double personHours = Double.Parse(row[2]);
                        // Store data
                        if (StoreMetric(productName, workActionId, personHours) == -1)
                            Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] Problem storing the resource utilization data to the database, please run the script again and make sure the database schema is correct. " + productDataPath);
                    }
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] product data file cannot properly be parsed due to its columns " + productDataPath);
                }
            } else
                Reporter.AddErrorMessageToReporter("[Metric 5: Resource Utilization] Unable to open product data file " + productDataPath);
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(string productName, int workActionId, double hours)
        {
            return DatabaseAccessor.WriteResourceUtilization(productName, hours, workActionId, iteration.IterationID);
        }

        /*internal bool EstablishConnection()
        {
            try {
                this.xlsReader.CheckConnection();
            } catch(Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }*/
    }
}
