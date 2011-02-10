using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using cpsc594_cdl.Common.Models;

namespace Importer_System
{
    public class CodeCoverage : Metric
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
        public int CalculateMetric(string project, string component, string file, int currIteration)
        {
            this.project = project;
            this.component = component;
            this.linesCovered = 0;
            this.linesExecuted = 0;
            this.file = file;
            this.iteration = currIteration;

            // Parse the file and return true if passed, false if error
            if(parseLogFile(file))
            {
                return StoreMetric();
            }
            else
                return -1;
        }

        /// <summary>
        ///     Returns the code coverage %
        /// </summary>
        /// <returns></returns>
        public double CodeCoverageValue()
        {
            try
            {
                return ((double)linesCovered / (double)linesExecuted) * 100;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
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
                    if (line.Substring(0, 2).Equals("SF"))
                    {
                        // Parse the Source File to see if it belongs in the Project and Component
                        string[] sf_directory = line.Split(':');
                        string directory = sf_directory[1];
                        string[] folders = directory.Split('/');
                        // Check the project name and component
                        if (project.Equals(folders[folders.Length - 3]) && component.Equals(folders[folders.Length - 2]))
                        {
                            Boolean foundCoverageLines = false;
                            String project_comp_line = line;
                            // Read to the end of the file
                            while ((line = file.ReadLine()) != null)
                            {
                                // If we cannot find any coverage results within the SF record, break the loop and return false
                                if (line.Equals("end_of_record") || line.Substring(0, 2).Equals("SF"))
                                    break;
                                // Found lines executed
                                if (line.Substring(0, 2).Equals("LF"))
                                {
                                    string[] lf_value = line.Split(':');
                                    int LF = Int16.Parse(lf_value[1]);
                                    line = file.ReadLine();
                                    // Found lines covered
                                    if (line.Substring(0, 2).Equals("LH"))
                                    {
                                        string[] lh_value = line.Split(':');
                                        int LH = Int16.Parse(lh_value[1]);
                                        linesExecuted += LF;
                                        linesCovered += LH;
                                        foundCoverageLines = true;
                                    }
                                }
                            }
                            // If the log is missing codecoverage files, throw an exception
                            if (!foundCoverageLines)
                                throw new InvalidDataException();
                        }
                    }
                }
            }
            catch (InvalidDataException)
            {
                // Missing lines in the report
                // Ex. Missing:
                //       - SF
                //       - LF
                //       - LH
                // Log the error
                Reporter.AddMessageToReporter("[Metric 1: Code Coverage] Missing lines in code coverage log file " + locationOfLog, true, false);
                saveResult = false;
            }
            catch (IndexOutOfRangeException)
            {
                // Log the error
                Reporter.AddMessageToReporter("[Metric 1: Code Coverage] Illegal syntax in code coverage log file " + locationOfLog, true, false);
                // Split function did not work, log file has invalid data
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
