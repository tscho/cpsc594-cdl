using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Importer_System;

namespace MetricAnalyzer.ImporterSystem.Tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass]
    public class CodeCoverageTest
    {
        public CodeCoverageTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /**---------------------------------------------------------------------------------
         * 
         *      CODE_COVERAGE_UNIT_TESTS
         * 
         *---------------------------------------------------------------------------------*/

        /// <summary>
        ///     Unit test verifying the correctness of the log file parser on various input.
        /// </summary>
        [TestMethod]
        public void CodeCoverageTest_ParseLog()
        {
            // Normal log file with multiple files - coverage > 0%
            Assert.AreEqual(9.59, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\normal_log.info", "Project1", "Comp1"));
            // Normal log with one file - coverage > 0%
            Assert.AreEqual(50, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\normal_log2.info", "Project1", "Comp1"));
            // Normal log with one file - coverage = 0%
            Assert.AreEqual(0, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\normal_log3.info", "Project1", "Comp1"));
            // Normal log with mixed components in the project - only choosing the right ones
            Assert.AreEqual(37.5, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\normal_log4.info", "Project1", "Comp1"));

            // Unexpected empty log
            Assert.AreEqual(0, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\unexpected_log.info", "Project1", "Comp1"));
            // Unexpected log with no SF relating to the Project and Component
            Assert.AreEqual(0, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\unexpected_log2.info", "Project1", "Comp1"));
            
            // Rare log with missing LH attribute
            Assert.AreEqual(-1, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\rare_log.info", "Project1", "Comp1"));
            // Rare log with missing LF attribute
            Assert.AreEqual(-1, parseLogFile("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\LogFiles\\rare_log2.info", "Project1", "Comp1"));
        }

        /// <summary>
        ///     Parses the log file and returns a value.
        ///     if value is positive = coverage
        ///     if value is negative = error
        /// </summary>
        /// <param name="locationOfLog"></param>
        /// <param name="project"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public Double parseLogFile(String locationOfLog, String project, String component)
        {
            StreamReader file = null;                       // Initialize file
            String line;                                    // Line used with StreamReader
            Double value = 0.0;
            try
            {
                double totalLines = 0;                      // Total lines of code of the component
                double totalLinesCovered = 0;               // Total lines covered of the component
                file = new StreamReader(locationOfLog);     // File stream of the log file
                // Read the log line by line
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Substring(0, 2).Equals("SF"))
                    {
                        // Parse the Source File to see if it belongs in the Project and Component
                        String[] sf_directory = line.Split(':');
                        String directory = sf_directory[1];
                        String[] folders = directory.Split('/');
                        if (project.Equals(folders[folders.Length - 3]) && component.Equals(folders[folders.Length - 2]))
                        {
                            Boolean foundCoverageLines = false;
                            while ((line = file.ReadLine()) != null)
                            {
                                if (line.Equals("end_of_record") || line.Substring(0, 2).Equals("SF"))
                                    break;
                                if (line.Substring(0, 2).Equals("LF"))
                                {
                                    String[] lf_value = line.Split(':');
                                    int LF = Int16.Parse(lf_value[1]);
                                    line = file.ReadLine();
                                    if (line.Substring(0, 2).Equals("LH"))
                                    {
                                        String[] lh_value = line.Split(':');
                                        int LH = Int16.Parse(lh_value[1]);
                                        totalLines += LF;
                                        totalLinesCovered += LH;
                                        foundCoverageLines = true;
                                    }
                                }
                            }
                            if (!foundCoverageLines)
                                throw new InvalidDataException();
                        }
                    }
                }
                // Fix division by zero
                if (totalLines == 0 && totalLinesCovered == 0)
                    value = 0;
                else
                    value = Math.Round(((totalLinesCovered / totalLines) * 100), 2);
            }
            catch (InvalidDataException)
            {
                // Missing lines in the report
                // Ex. Missing:
                //       - LF
                //       - LH
                return -1;
            }
            catch (IndexOutOfRangeException)
            {
                // Split function did not work, log file has invalid data
                return -2;
            }
            finally
            {
                try { file.Close(); } catch (Exception) { }
            }
            return value;
        }
    }
}
