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

namespace Importer_System_Tests
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
          
            String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
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

            //Setup for renameFileTest
            fs = File.Create(metricDirectory + "\\" + "curr.info");
            fs.Close();
            
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            String archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            String metricDirectory = ConfigurationManager.AppSettings["rootDirectory"] + "\\" + "Project1" + "\\" + "Comp1" + "\\" + "Metric1";
            //Cleanup for ArchiveFileTest()
            File.Delete(archDirectory + "\\" + "normal.info");

            //Cleanup for  UpdateArchiveDirectoryTest()
            archDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"] + "\\" + "Project1" + "\\" + "Comp2" + "\\" + "Metric1";
            File.Delete(archDirectory + "\\" + "arch2.info");
            File.Delete(archDirectory + "\\" + "arch3.info");

            //Setup for renameFileTest
            File.Delete(metricDirectory + "\\" + "new.info");
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
        ///     Unit test verifying that the importer scans through the given directory
        /// </summary>
        [TestMethod()]
        public void ImporterEngineTestIterateDirectory()
        {
            // Sample directory no projects
            String sample_one = "";
            Assert.AreEqual(sample_one, BeginImporting("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\Directory\\Directory1"));
            // Sample directory projects no components
            String sample_two = "Project1Project2Project3";
            Assert.AreEqual(sample_two, BeginImporting("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\Directory\\Directory2"));
            // Sample directory projects with components
            String sample_three = "Project1Comp1Comp2Project2Project3";
            Assert.AreEqual(sample_three, BeginImporting("C:\\Users\\Russ\\Documents\\Visual Studio 2010\\Projects\\ImporterSystem\\Importer_System_tests\\Directory\\Directory3"));
        }

        /// <summary>
        ///     Scans directory.
        /// </summary>
        public String BeginImporting(String rootDirectory)
        {
            
            String list = "";
            // Make directory structure
            DirectoryInfo initialDirectory = new DirectoryInfo(rootDirectory);
            // Iterate through the projects in the directory
            if (initialDirectory.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo project in initialDirectory.GetDirectories())
                {
                    // Save the current projects name
                    String currentProjectName = project.Name;
                    list += currentProjectName;
                    // Iterate through the selected projects components
                    foreach (DirectoryInfo component in project.GetDirectories())
                    {
                        String currentComponentName = component.Name;
                        list += currentComponentName;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Unit test for ArchiveFile.
        /// Dependencies:
        /// 1. normal.info file falls withing the metricDirectory
        /// 2. doesnotexist.info is a file that does not exist
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Importer_System.exe")]
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
        [DeploymentItem("Importer_System.exe")]
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
            List<string> expected = null; // TODO: Initialize to an appropriate value
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
            DateTime day = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = ImportEngine.GetWeekNumber(day);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetIterationStart
        ///</summary>
        [TestMethod()]
        public void GetIterationStartTest()
        {
            DateTime endOfLastIteration = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = ImportEngine.GetIterationStart(endOfLastIteration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetIterationEnd
        ///</summary>
        [TestMethod()]
        public void GetIterationEndTest()
        {
            DateTime beginOfCurrIteration = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = ImportEngine.GetIterationEnd(beginOfCurrIteration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FirstMondayOfYear
        ///</summary>
        [TestMethod()]
        public void FirstMondayOfYearTest()
        {
            int year = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = ImportEngine.FirstMondayOfYear(year);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DetermineIterationLetter
        ///</summary>
        [TestMethod()]
        public void DetermineIterationLetterTest()
        {
            ImportEngine target = new ImportEngine(); // TODO: Initialize to an appropriate value
            DateTime iterationStart = new DateTime(); // TODO: Initialize to an appropriate value
            char expected = '\0'; // TODO: Initialize to an appropriate value
            char actual;
            actual = target.DetermineIterationLetter(iterationStart);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for findNewProducts
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Importer_System.exe")]
        public void findNewProductsTest()
        {
            ImportEngine_Accessor target = new ImportEngine_Accessor(); // TODO: Initialize to an appropriate value
            string file = string.Empty; // TODO: Initialize to an appropriate value
            target.findNewProducts(file);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
