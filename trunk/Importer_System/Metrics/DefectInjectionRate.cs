using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Importer_System.Util;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Importer_System
{
    class DefectInjectionRate : Metric
    {
        private string bugzillaConnectionString;
        private string project;
        private string component;
        private int numberOfHighDefects;
        private int numberOfMediumDefects;
        private int numberOfLowDefects;
        private Iteration iteration;
        private SqlConnection connection;

        public DefectInjectionRate(string conString)
        {
            bugzillaConnectionString = conString;
        }

        /// <summary>
        ///     Using the connection string given, return true if a connection was established, false otherwise.
        /// </summary>
        /// <returns></returns>
        public Boolean EstablishConnection()
        {
            connection = new SqlConnection(bugzillaConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Basic metric function that calculates the defect injection rate with three query calls.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="component"></param>
        public void CalculateMetric(String project, String component, Iteration currIteration)
        {
            this.project = project;
            this.component = component;
            this.numberOfHighDefects = 0;
            this.numberOfMediumDefects = 0;
            this.numberOfLowDefects = 0;
            this.iteration = currIteration;

            // --------------------------------------
            // Count the number of minor bugs - LOW
            // --------------------------------------
            SqlCommand cmd = new SqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'minor'", connection);
            SqlDataReader myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfLowDefects++;

            }
            myReader.Close();
            // --------------------------------------
            // Count the number of major bugs - MEDIUM
            // --------------------------------------
            cmd = new SqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'major'", connection);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfMediumDefects++;

            }
            myReader.Close();
            // --------------------------------------
            // Count the number of critical bugs - HIGH
            // --------------------------------------
            cmd = new SqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'critical'", connection);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfHighDefects++;

            }
            myReader.Close();
            // Store the results
            StoreMetric();
        }

        /// <summary>
        ///     Call to the database class to properly store the information.
        /// </summary>
        public int StoreMetric()
        {
            // Call to database to store
            return Database.WriteDefectInjectionRate(project, component, numberOfHighDefects, numberOfMediumDefects, numberOfLowDefects, iteration.IterationID);
        }
        
        /// <summary>
        ///     Compares the date with the start and end date, returns true if the date is within or equal bounds.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool IsBetween(DateTime startDate, DateTime endDate, DateTime date)
        {
            return (startDate.CompareTo(date) <= 0 && endDate.CompareTo(date) >= 0);
        }
    }
}
