﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Windows.Forms;

namespace Importer_System
{
    class DefectMetrics : Metric
    {
        private string bugzillaConnectionString;
        private string project;
        private string component;
        private int numberOfHighDefects;
        private int numberOfMediumDefects;
        private int numberOfLowDefects;
        private int numberOfVerifiedDefects;
        private int numberOfResolvedDefects;
        private Iteration iteration;
        private MySqlConnection connection;

        public DefectMetrics(string conString)
        {
            bugzillaConnectionString = conString;
        }

        /// <summary>
        ///     Using the connection string given, return true if a connection was established, false otherwise..
        /// </summary>
        /// <returns></returns>
        public Boolean EstablishConnection()
        {
            try { connection = new MySqlConnection(bugzillaConnectionString); } catch { return false; }
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
            this.iteration = currIteration;

            // -------------------------------------------
            // CALCULATE METRIC 3 - Defect Injection Rate
            // -------------------------------------------

            // Variables for metric 3
            this.numberOfHighDefects = 0;
            this.numberOfMediumDefects = 0;
            this.numberOfLowDefects = 0;

            // --------------------------------------
            // Count the number of minor bugs - LOW
            // --------------------------------------
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'minor'", connection);
            MySqlDataReader myReader = cmd.ExecuteReader();
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
            cmd = new MySqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'major'", connection);
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
            cmd = new MySqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'CONFIRMED' AND bug_severity = 'critical'", connection);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfHighDefects++;

            }
            myReader.Close();

            // -------------------------------------------
            // CALCULATE METRIC 4 - Defect Repair Rate
            // -------------------------------------------

            // Variables for metric 4
            this.numberOfVerifiedDefects = 0;
            this.numberOfResolvedDefects = 0;

            // --------------------------------------
            // Count the number of verified defects
            // --------------------------------------
            cmd = new MySqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'VERIFIED'", connection);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfVerifiedDefects++;

            }
            myReader.Close();

            // --------------------------------------
            // Count the number of resolved defects
            // --------------------------------------
            cmd = new MySqlCommand("SELECT * FROM Bugs WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'RESOLVED'", connection);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                DateTime bugDate = myReader.GetDateTime(8);
                if (IsBetween(currIteration.StartDate, currIteration.EndDate, bugDate))
                    numberOfResolvedDefects++;

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
            DatabaseAccessor.WriteDefectInjectionRate(project, component, numberOfHighDefects, numberOfMediumDefects, numberOfLowDefects, iteration.IterationID);
            DatabaseAccessor.WriteDefectRepairRate(project, component, numberOfVerifiedDefects, numberOfResolvedDefects, iteration.IterationID);
            return -1;
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
