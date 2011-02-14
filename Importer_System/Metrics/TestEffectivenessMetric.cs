using System;
using System.IO;
using cpsc594_cdl.Common.Models;

namespace Importer_System.Metrics
{
    internal class TestEffectivenessMetric : Metric
    {
        private string file; // File directory
        private int iteration;
        private string project; // The project assigned


        /// <summary>
        ///     Calculates the given log file 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        /// <returns>True if no errors</returns>
        public void CalculateMetric(string file, int currIteration, String project)
        {
            this.file = file;
            this.iteration = currIteration;
            this.project = project;

            // Parse the file and return true if passed, false if error
            parseLogFile(file);
        }

        /// <summary>
        ///     Opens the coverage log file and calculates the code coverage
        /// </summary>
        /// <param name="locationOfLog">Directory path of the log file</param>
        private Boolean parseLogFile(string locationOfLog)
        {
            int expectedPass = 0;
            int unexpectedFail = 0;
            int unexpectedSucc = 0;
            int expectedFail = 0;
            int unsupportedTests = 0;
            int untestedTestcases = 0;
            Boolean writtenToDB = false;
            int id = 0;
            String component; // The component assigned
            String tempComponent;
            String[] summary;

            StreamReader file = null; // Initialize file
            string line; // Line used with StreamReader
            Boolean saveResult = true; // Save the metric at the end of the algorithm
            try
            {
                file = new StreamReader(locationOfLog); // File stream of the log file
                // Read the log line by line
                while ((line = file.ReadLine()) != null)
                {
                    if (!line.Contains("Summary"))
                    {
                        continue;
                    }
                    else
                    {
                        summary = line.Split(' ');
                        component = summary[1];
                    }

                    while ((line = file.ReadLine()) != null)
                    {
                        // If we cannot find any coverage results within the SF record, break the loop and return false
                        if (line.Contains("tests ==="))
                        {
                            if(writtenToDB == false)
                            {
                                StoreMetric(component,
                                            expectedFail + expectedPass + unexpectedSucc + unexpectedFail +
                                            unsupportedTests);
                            }
                            expectedFail = 0;
                            expectedPass = 0;
                            unexpectedSucc = 0;
                            unexpectedFail = 0;
                            unsupportedTests = 0;
                            untestedTestcases = 0;
                            break;
                        }
                        // Found lines executed
                        if (line.Length == 0 || (!line.Substring(0, 1).Equals("#") && !line.Substring(0,1).Equals("/")))
                        {
                            continue;
                        }

                        if (line.Contains("expected passes"))
                        {
                            expectedPass = FindLineValue(line);
                        }
                        else if (line.Contains("unexpected failures"))
                        {
                            unexpectedFail = FindLineValue(line);
                        }
                        else if (line.Contains("unexpected successes"))
                        {
                            unexpectedSucc = FindLineValue(line);
                        }
                        else if (line.Contains("expected failures	"))
                        {
                            expectedFail = FindLineValue(line);
                        }
                        else if (line.Contains("unsupported tests"))
                        {
                            unsupportedTests = FindLineValue(line);
                        }
                        else if (line.Contains("untested testcases"))
                        {
                            untestedTestcases = FindLineValue(line);
                        }
                        else
                        {
                            //parse path to find project and component
                            tempComponent = FindComponent(line);
                            if(tempComponent != null)
                            {
                                component = FindComponent(line);
                                id = StoreMetric(component,
                                            expectedFail + expectedPass + unexpectedSucc + unexpectedFail +
                                            unsupportedTests + untestedTestcases);
                                if (id != -1)
                                    writtenToDB = true;
                            }
                        }
                    }
                }
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

        private int FindLineValue(String line)
        {
            string lineValue;
            string[] splitLine;
            splitLine = line.Split('\t');
            lineValue = splitLine[splitLine.Length - 1];
            return Convert.ToInt32(lineValue);
        }

        private String FindComponent(String line)
        {
            string lineValue;
            string[] directories;
            string[] componentLine;
            String component = null;
            int index = 0;
            directories = line.Split('/');
            foreach (String directory in directories)
            {     
                if(directory == this.project)
                {
                    component = directories[index + 1];
                    componentLine = component.Split(' ');
                    if(componentLine.Length > 1)
                        component = componentLine[0];
                }
                index++;
            }
            return component;
        }

        /// <summary>
        ///     Database call to store the results.
        /// </summary>
        public int StoreMetric(String component, int testCases)
        {
            return DatabaseAccessor.WriteTestEffectiveness(this.project, component, testCases, iteration);
        }
    }
}