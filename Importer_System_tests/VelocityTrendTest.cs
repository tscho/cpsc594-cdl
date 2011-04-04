﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Importer_System;

namespace Importer_System_tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass]
    public class VelocityTrendTest
    {
        public VelocityTrendTest()
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
         *      VelocityTrendTest
         * 
         *---------------------------------------------------------------------------------*/

        /// <summary>
        ///     Unit test verifying the correctness of the log file parser on various input.
        /// </summary>
        [TestMethod]
        public void CalculateVelocityTrendTest()
        {
            // Normal data
            List<string[]> data = CalculateVelocityTrend("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\normal.xls");
            string[] row = data[0];
            Assert.AreEqual(18, double.Parse(row[1]));
            Assert.AreEqual(6.1, double.Parse(row[2]));
            // No data
            data = CalculateVelocityTrend("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\nodata.xls");
            Assert.AreEqual(0, data.Count);
            // Rare data
            data = CalculateVelocityTrend("09-E", "C:\\Users\\Russ\\Desktop\\ProductData\\rare_vel.xls");
            row = data[0];
            Assert.AreEqual(19, Int16.Parse(row[1]));
            Assert.AreEqual(6.2, double.Parse(row[2]));
        }
        private List<string[]> CalculateVelocityTrend(string iterationLabel, string productDataPath)
        {
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + productDataPath + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select [Product], Sum([Estimate]), Sum([Actual]) from [Sheet1$] WHERE [Iteration]='",
                                  iterationLabel, "' and [Scope]='True' GROUP BY [Product]");
                    return xlsReader.SelectQuery(query);

                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
    }
}