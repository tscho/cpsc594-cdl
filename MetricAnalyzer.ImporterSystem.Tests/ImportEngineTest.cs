using System;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Importer_System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;
using MetricAnalyzer.Common.Models;
using MetricAnalyzer.ImporterSystem;

namespace MetricAnalyzer.ImportSystem.Tests
{
    /// <summary>
    /// Summary description for AllTests
    /// </summary>
    [TestClass()]
    public class ImportEngineTest
    {
        public ImportEngineTest()
        {
         
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

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
          
/*            String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            String archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";

            //Setup for ArchiveFileTest()
            FileStream fs = File.Create(metricDirectory + "\\" + "normal.info");
            fs.Close();

            //Setup for  UpdateArchiveDirectoryTest()
            DateTime curr;
            TimeSpan old = new TimeSpan(95, 0, 0, 0); //archive days is set to 90
            TimeSpan zero = new TimeSpan(0, 0, 0, 0);
            TimeSpan newer = new TimeSpan(45, 0, 0, 0);
            curr = DateTime.UtcNow;
            archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp2" + "\\" + "Metric1";
            fs = File.Create(archDirectory + "\\" + "arch1.info");
            fs.Close();
            fs = File.Create(archDirectory + "\\" + "arch2.info");
            fs.Close();
            fs = File.Create(archDirectory + "\\" + "arch3.info");
            fs.Close();
            File.SetLastWriteTimeUtc(archDirectory + "\\" + "arch1.info", curr - old);
            File.SetLastWriteTimeUtc(archDirectory + "\\" + "arch2.info", curr - zero);
            File.SetLastWriteTimeUtc(archDirectory + "\\" + "arch3.info", curr - newer);
            */
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
/*            String archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            //Cleanup for ArchiveFileTest()
            File.Delete(archDirectory + "\\" + "normal.info");

            //Cleanup for  UpdateArchiveDirectoryTest()
            archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp2" + "\\" + "Metric1";
            File.Delete(archDirectory + "\\" + "arch2.info");
            File.Delete(archDirectory + "\\" + "arch3.info");*/
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
         *      IMPORTER_ENGINE_UNIT_TESTS
         * 
         *---------------------------------------------------------------------------------*/



        /// <summary>
        /// Unit test for ArchiveFile.
        /// Dependencies:
        /// 1. normal.info file falls withing the metricDirectory
        /// 2. doesnotexist.info is a file that does not exist
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MetricAnalyzer.ImporterSystem.exe")]
        public void ArchiveFileTest()
        {
            ImportEngine_Accessor target = new ImportEngine_Accessor(); // TODO: Initialize to an appropriate value

            string project = "Project1";
            string component = "Comp1";
            string normalFile = "normal.info"; 
            string dneFile = "doesnotexist.info";

            String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + project + "\\" + component + "\\" + "Metric1";
            String archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + project + "\\" + component + "\\" + "Metric1";

            bool actual = target.ArchiveFile(project, component, normalFile);
            Assert.AreEqual(File.Exists(archDirectory + "\\" + normalFile), actual);
            actual = target.ArchiveFile(project, component, dneFile); 
            Assert.AreEqual(File.Exists(archDirectory + "\\" + dneFile), actual); 
        }



        /// <summary>
        /// Unit test for UpdateArchiveDirectoryTest.
        /// Dependencies:
        /// 
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MetricAnalyzer.ImporterSystem.exe")]
        public void UpdateArchiveDirectoryTest()
        {
            String[] files;
            //String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            String archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp2" + "\\" + "Metric1";

            ImportEngine_Accessor target = new ImportEngine_Accessor();

            files = Directory.GetFiles(archDirectory);
            Assert.AreEqual(files.Length, 3);
            target.UpdateArchiveDirectory();
            files = Directory.GetFiles(archDirectory);
            Assert.AreEqual(files.Length, 2);
        }

        /// <summary>
        ///A test for GetListOfProducts
        ///</summary>
        [TestMethod()]
        public void GetListOfProductsTest()
        {
            ImportEngine target = new ImportEngine(); // TODO: Initialize to an appropriate value
            List<string> expected = null;
            List<string> actual;
            actual = target.GetListOfProducts();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetWeekNumber
        ///</summary>
        [TestMethod()]
        public void GetWeekNumberTest()
        {
            int actual;

            //First first monday of year
            DateTime day = new DateTime(2011, 1, ImportEngine.FirstMondayOfYear(2011));
            actual = ImportEngine.GetWeekNumber(day);
            Assert.AreEqual(1, actual);

            //Middle of year
            day = new DateTime(2011, 6, 15);
            actual = ImportEngine.GetWeekNumber(day);
            Assert.AreEqual(24, actual);

            //Last day of year
            day = new DateTime(2011,12,31);
            actual = ImportEngine.GetWeekNumber(day);
            Assert.AreEqual(52, actual);
        }

        /// <summary>
        ///A test for GetIterationStart
        ///</summary>
        [TestMethod()]
        public void GetIterationStartTest()
        {
            //Last friday of year
            DateTime endOfLastIteration = new DateTime(2011, 12, 30); 
            DateTime expected = new DateTime(2012,01,02);
            DateTime actual;
            actual = ImportEngine.GetIterationStart(endOfLastIteration);
            Assert.AreEqual(expected, actual);

            //First friday of year
            endOfLastIteration = new DateTime(2011, 1, 7);
            expected = new DateTime(2011, 1, 10);
            actual = ImportEngine.GetIterationStart(endOfLastIteration);
            Assert.AreEqual(expected, actual);

            //Last day of year is midweek (next business day)
            endOfLastIteration = new DateTime(2012, 12, 31);
            expected = new DateTime(2013, 1, 1);
            actual = ImportEngine.GetIterationStart(endOfLastIteration);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetIterationEnd
        ///</summary>
        [TestMethod()]
        public void GetIterationEndTest()
        {
            //Start at last monday of year (should be last day of year)
            DateTime beginOfCurrIteration = new DateTime(2011, 12, 26); 
            DateTime expected = new DateTime(2011,12,30);
            DateTime actual;
            actual = ImportEngine.GetIterationEnd(beginOfCurrIteration);
            Assert.AreEqual(expected, actual);

            //Regular iteration start
            beginOfCurrIteration = new DateTime(2011, 1, 1);
            expected = new DateTime(2011, 1, 14); 
            actual = ImportEngine.GetIterationEnd(beginOfCurrIteration);
            Assert.AreEqual(expected, actual);

            //Regular iteration start
            beginOfCurrIteration = new DateTime(2013, 1, 1);
            expected = new DateTime(2013, 1, 11);
            actual = ImportEngine.GetIterationEnd(beginOfCurrIteration);
            Assert.AreEqual(expected, actual);

            //Start at 2nd last monday of year (should be last day of year)
            beginOfCurrIteration = new DateTime(2012,12,24);
            expected = new DateTime(2012,12,31); 
            actual = ImportEngine.GetIterationEnd(beginOfCurrIteration);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FirstMondayOfYear
        ///</summary>
        [TestMethod()]
        public void FirstMondayOfYearTest()
        {
            //2011
            int year = 2011;
            int expected = 3; 
            int actual;
            actual = ImportEngine.FirstMondayOfYear(year);
            Assert.AreEqual(expected, actual);
            
            //2012
            year = 2012;
            expected = 2;
            actual = ImportEngine.FirstMondayOfYear(year);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DetermineIterationLetter
        ///</summary>
        [TestMethod()]
        public void DetermineIterationLetterTest()
        {
            ImportEngine target = new ImportEngine(); 
            DateTime iterationStart = new DateTime(2011,1,3); 
            char expected = 'A';
            char actual;
            actual = target.DetermineIterationLetter(iterationStart);
            Assert.AreEqual(expected, actual);

            iterationStart = new DateTime(2011,1,17);
            expected = 'B';
            actual = target.DetermineIterationLetter(iterationStart);
            Assert.AreEqual(expected, actual);

            iterationStart = new DateTime(2011, 1, 19);
            expected = 'B';
            actual = target.DetermineIterationLetter(iterationStart);
            Assert.AreEqual(expected, actual);

            iterationStart = new DateTime(2011, 12, 26);
            expected = 'Z';
            actual = target.DetermineIterationLetter(iterationStart);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for findNewProducts
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MetricAnalyzer.ImporterSystem.exe")]
        public void findNewProductsTest()
        {
            ImportEngine_Accessor target = new ImportEngine_Accessor(); // TODO: Initialize to an appropriate value
            string file = string.Empty; // TODO: Initialize to an appropriate value
            target.findNewProducts(file);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateIteration
        ///</summary>
        [TestMethod()]
        public void UpdateIterationTest()
        {
            ImportEngine target = new ImportEngine(); // TODO: Initialize to an appropriate value
            Iteration expected = null; // TODO: Initialize to an appropriate value
            Iteration actual;
            actual = target.UpdateIteration();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
