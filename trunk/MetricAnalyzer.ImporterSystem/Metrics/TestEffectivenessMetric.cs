using System;
using System.IO;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.ImporterSystem.Metrics
{
    internal class TestEffectivenessMetric : Metric
    {
        private string file; // File directory
        private int iteration;
        private string product; // The product assigned
        int _testsExecuted = 0;


        /// <summary>
        ///     Calculates the given log file 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="file"></param>
        /// <returns>True if no errors</returns>
        public int CalculateMetric(string file, int currIteration, String product)
        {
            this.file = file;
            this.iteration = currIteration;
            this.product = product;

            // Parse the file and return true if passed, false if error
            if (parseLogFile(file))
                return StoreMetric(product, _testsExecuted);
            else
                return -1;
       }

        /// <summary>
        ///     Opens the coverage log file and calculates the code coverage
        /// </summary>
        /// <param name="locationOfLog">Directory path of the log file</param>
        private Boolean parseLogFile(string locationOfLog)
        {
            StreamReader file = null; // Initialize file
            string line; // Line used with StreamReader
            Boolean saveResult = true; // Save the metric at the end of the algorithm

            _testsExecuted = 0;
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

                    while ((line = file.ReadLine()) != null)
                    {
                        // If we cannot find any coverage results within the SF record, break the loop and return false
                        if (line.Contains("tests ==="))
                        {
                            break;
                        }
                        // Found lines executed
                        if (line.Length == 0 || !line.Substring(0, 1).Equals("#"))
                        {
                            continue;
                        }
                        else
                        {
                            _testsExecuted += FindLineValue(line);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Log the error
                Reporter.AddErrorMessageToReporter("[Metric 1: Code Coverage] Illegal syntax in code coverage log file " + locationOfLog);
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
            string[] directories;
            string[] componentLine;
            String component = null;
            int index = 0;
            directories = line.Split('/');
            foreach (String directory in directories)
            {     
                if(directory == this.product)
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
        public int StoreMetric(String product, int testCases)
        {
            return DatabaseAccessor.WriteTestEffectiveness(this.product, testCases, iteration);
        }
    }
}