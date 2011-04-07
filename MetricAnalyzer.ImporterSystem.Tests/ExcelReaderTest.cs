using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Importer_System;
using System.Windows.Forms;

namespace MetricAnalyzer.ImporterSystem.Tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass]
    public class ExcelReaderTest
    {
        public ExcelReaderTest()
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
         *      EXCEL READER TESTS
         * 
         *---------------------------------------------------------------------------------*/

        /// <summary>
        ///     Unit test verifying the correctness of the connection.
        /// </summary>
        [TestMethod]
        public void CheckConnectionTest()
        {
            Assert.AreEqual(true, CheckConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Russ\\Desktop\\ProductData\\projectdata.xls;Extended Properties=Excel 5.0"));
            Assert.AreEqual(false, CheckConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Russ\\Desktop\\ProductData\\doesnotexist.xls;Extended Properties=Excel 5.0"));
        }

        /// <summary>
        ///     CheckConnection - throws an exception which is handled above in resourceUtilization if no connect is made
        /// </summary>
        public Boolean CheckConnection(string connectionString)
        {
            try
            {
                System.Data.OleDb.OleDbConnection ExcelConnection = new System.Data.OleDb.OleDbConnection(connectionString);
                System.Data.OleDb.OleDbCommand ExcelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [Sheet1$]", ExcelConnection);
                ExcelConnection.Open();
                System.Data.OleDb.OleDbDataReader ExcelReader;
                ExcelReader = ExcelCommand.ExecuteReader();
                ExcelReader.Read();
                ExcelConnection.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Unit test verifying the correctness of the excel reader query library.
        /// </summary>
        [TestMethod]
        public void RunQueryTest()
        {
            List<string[]> data = SelectQuery("Select [Product] from [Sheet1$]", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Russ\\Desktop\\ProductData\\excelreadertest.xls;Extended Properties=Excel 5.0");
            if (data == null)
                Assert.Fail("No data returned from query");
            else
            {
                string[] row = data[0];
                Assert.AreEqual("php", row[0]);
            }
        }

        /// <summary>
        ///     SelectQuery - takes in a query and executes it, returns a list of string, each results it on a line with a comma delimeter
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List<String[]></returns>
        public List<string[]> SelectQuery(string query, string connectionString)
        {
            try
            {
                System.Data.OleDb.OleDbConnection ExcelConnection = new System.Data.OleDb.OleDbConnection(connectionString);

                System.Data.OleDb.OleDbCommand ExcelCommand = new System.Data.OleDb.OleDbCommand(query, ExcelConnection);
                ExcelConnection.Open();
                System.Data.OleDb.OleDbDataReader ExcelReader;

                ExcelReader = ExcelCommand.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (ExcelReader.Read())
                {
                    string[] columnData = new string[ExcelReader.FieldCount];
                    for (int i = 0; i < ExcelReader.FieldCount; i++)
                    {
                        columnData[i] = ExcelReader.GetValue(i).ToString();
                    }
                    data.Add(columnData);
                }
                ExcelConnection.Close();
                return data;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
