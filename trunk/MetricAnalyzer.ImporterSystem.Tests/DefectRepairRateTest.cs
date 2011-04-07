using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Importer_System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace MetricAnalyzer.ImporterSystem.Tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass]
    public class DefectRepairRateTest
    {
        public DefectRepairRateTest()
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
         *      DefectRepairRateTest Unit Tests
         * 
         *---------------------------------------------------------------------------------*/

        /// <summary>
        ///     Unit test verifying the connection string makes a persistent connection to the database.
        /// </summary>
        [TestMethod]
        public void DefectInjectionRateTest_ConnectDatabase()
        {
            String bugzillaConnectionString = "Data Source=localhost;Database=Bugzilla;User ID=root;Password="; // Connection string from config file
            MySqlConnection connection = new MySqlConnection(bugzillaConnectionString);
            try
            {
                connection.Open();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        ///     Unit test verifying that the queries return the correct results.
        /// </summary>
        [TestMethod]
        public void DefectRepairRateTest_CalculateBugs()
        {
            String bugzillaConnectionString = "Data Source=localhost;Database=Bugzilla;User ID=root;Password="; // Connection string from config file
            MySqlConnection connection = new MySqlConnection(bugzillaConnectionString);
            try
            {
                connection.Open();
                // --------------------------------------
                // Count the number of verified defects
                // --------------------------------------
                int numberOfVerifiedDefects = 0;
                string project = "project1";
                string component = "component1";
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM UnitTest_0003 WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'VERIFIED'", connection);
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    DateTime bugDate = myReader.GetDateTime(8);
                    if (IsBetween(Tdt("2010-09-15"), Tdt("2010-09-24"), bugDate))
                        numberOfVerifiedDefects++;

                }
                myReader.Close();
                Assert.AreEqual(1, numberOfVerifiedDefects);

                // --------------------------------------
                // Count the number of resolved defects
                // --------------------------------------
                int numberOfResolvedDefects = 0;
                cmd = new MySqlCommand("SELECT * FROM UnitTest_0003 WHERE product = '" + project + "' AND component = '" + component + "' AND bug_status = 'RESOLVED'", connection);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    DateTime bugDate = myReader.GetDateTime(8);
                    if (IsBetween(Tdt("2010-09-15"), Tdt("2010-09-24"), bugDate))
                        numberOfResolvedDefects++;

                }
                myReader.Close();
                Assert.AreEqual(1, numberOfResolvedDefects);
            }
            catch (SqlException)
            {
                Assert.IsTrue(false);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        ///     Function to be tested.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool IsBetween(DateTime startDate, DateTime endDate, DateTime date)
        {
            return (startDate.CompareTo(date) <= 0 && endDate.CompareTo(date) >= 0);
        }

        /// <summary>
        ///     Helper to convert string dates to datetime format.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime Tdt(String date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", null);
        }
    }
}
