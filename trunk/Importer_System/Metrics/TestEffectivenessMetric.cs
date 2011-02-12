using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cpsc594_cdl.Common.Models;

namespace Importer_System.Metrics
{
    class TestEffectivenessMetric : Metric
    {

        private string project;     // The project assigned
        private string component;   // The component assigned
        private int linesCovered;   // The lines covered
        private int linesExecuted;  // The lines found
        private string file;        // File directory
        private int iteration;


        /// <summary>
        ///     Calculates the given log file 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        /// <returns>True if no errors</returns>
        public int CalculateMetric(string file, int currIteration)
        {
            this.file = file;
            this.iteration = currIteration;

            // Parse the file and return true if passed, false if error
            if (parseLogFile(file))
            {
                return StoreMetric();
            }
            else
                return -1;
        }

        /// <summary>
        ///     Opens the coverage log file and calculates the code coverage
        /// </summary>
        /// <param name="locationOfLog">Directory path of the log file</param>
        private Boolean parseLogFile(string locationOfLog)
        {
            StreamReader file = null;                       // Initialize file
            string line;                                    // Line used with StreamReader
            Boolean saveResult = true;                      // Save the metric at the end of the algorithm
            try
            {
                file = new StreamReader(locationOfLog);     // File stream of the log file
                // Read the log line by line
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Summary"))
                    {
                        line = file.ReadLine();

                    }
                }
            }
            catch (InvalidDataException)
            {
                saveResult = false;
            }
            catch (IndexOutOfRangeException)
            {
                saveResult = false;
            }
            finally
            {
                file.Close();
            }
            return saveResult;
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric()
        {
            return DatabaseAccessor.WriteCodeCoverage(project, component, linesCovered, linesExecuted, iteration);

        }
    }
}
