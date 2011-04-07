using System;
using MetricAnalyzer.Common.Models;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetricAnalyzer.ImporterSystem;

namespace MetricAnalyzer.ImporterSystem.Tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass]
    public class ReworkTest
    {
        public ReworkTest()
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
         *      REWORK_METRIC_TESTS
         * 
         *---------------------------------------------------------------------------------*/

        /// <summary>
        ///     Unit test verifying the correctness of the log file parser on various input.
        /// </summary>
        [TestMethod]
        public void CalculateReworkTest()
        {
            // Normal data
            Assert.AreEqual(6.1, CalculateRework("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\normal.xls", "php-5.3.5"));
            // No data
            Assert.AreEqual(0.0, CalculateRework("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\nodata.xls", "php-5.3.5"));
            // No data
            Assert.AreEqual(10.5, CalculateRework("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\rare_rework.xls", "php-5.3.5"));
        }
        /// <summary>
        ///     CalculateRework
        /// </summary>
        /// <param name="iterationLabel"></param>
        /// <param name="productDataPath"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        private double CalculateRework(string iterationLabel, string productDataPath, string productName)
        {
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + productDataPath + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], [Work Action ID], Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                      iterationLabel, "' AND [Product]='", productName, "' GROUP BY [Product], [Work Action ID]");
                    List<string[]> data = xlsReader.SelectQuery(query);
                    double sumRework = 0.0;
                    foreach (string[] row in data)
                    {
                        int workActionId = Int16.Parse(row[1]);
                        double reworkHours = Double.Parse(row[2]);
                        // Store data
                        if (DetermineIfRework(workActionId, productName))
                        {
                            sumRework += reworkHours;
                        }
                    }
                    return sumRework;
                }
                catch
                {
                    return 0.0;
                }
            }
            else
                return 0.0;
        }
        /// <summary>
        ///     DetermineIfRework - handler
        /// </summary>
        /// <param name="workActionId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool DetermineIfRework(int workActionId, string product)
        {
            return true;
        }
    }
}
