using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Windows.Forms;

namespace Importer_System.Metrics
{
    class ResourceUtilization
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
                    List<string[]> workHours = xlsReader.SelectQuery("Select [Product], [Person Name], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='09-E' GROUP BY [Product], [Person Name]");
                    foreach (string[] row in workHours)
                    {
                        string projectName = row[0];
                        string personName = row[1];
                        double personHours = Double.Parse(row[2]);
                        // Store data
                        StoreMetric(projectName, personName, personHours);
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
        public int StoreMetric(string project, string personName, double hours)
        {
            DatabaseAccessor.WriteResourceUtilization(project, personName, hours, iteration.IterationID);
            return -1;
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
