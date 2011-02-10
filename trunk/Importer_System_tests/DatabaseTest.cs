using Importer_System;
using Importer_System.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Importer_System_Tests
{
    
    
    /// <summary>
    ///This is a test class for DatabaseTest and is intended
    ///to contain all DatabaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabaseTest
    {


        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ComponentExists
        ///</summary>
        [TestMethod()]
        public void ComponentExistsTest()
        {
            string projectName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Database.ComponentExists(projectName, componentName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetLastIteration
        ///</summary>
        [TestMethod()]
        public void GetLastIterationTest()
        {
            Iteration expected = null; // TODO: Initialize to an appropriate value
            Iteration actual;
            actual = Database.GetLastIteration();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ProjectExists
        ///</summary>
        [TestMethod()]
        public void ProjectExistsTest()
        {
            string projectName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Database.ProjectExists(projectName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateCoverage
        ///</summary>
        [TestMethod()]
        public void UpdateCoverageTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Database.UpdateCoverage(id, date, fileName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteCodeCoverage
        ///</summary>
        [TestMethod()]
        public void WriteCodeCoverageTest()
        {
            string projectName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            int linesCovered = 0; // TODO: Initialize to an appropriate value
            int linesExecuted = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            int iteration = 1;
            actual = Database.WriteCodeCoverage(projectName, componentName, linesCovered, linesExecuted, iteration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteComponent
        ///</summary>
        [TestMethod()]
        public void WriteComponentTest()
        {
            string projectName = "WriteProjectTest"; // TODO: Initialize to an appropriate value
            string componenetName = "WriteComponentTest"; // TODO: Initialize to an appropriate value
            Database.WriteComponent(projectName, componenetName);
            Assert.AreEqual(true, Database.ComponentExists(projectName, componenetName));
            Database.WriteComponent(projectName, "");
            Assert.AreEqual(false, Database.ComponentExists(projectName, ""));
            Database.WriteComponent(projectName, componenetName);
            Assert.AreEqual(false, Database.ComponentExists("", componenetName));
        }

        /// <summary>
        ///A test for WriteIteration
        ///</summary>
        [TestMethod()]
        public void WriteIterationTest()
        {
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            Database.WriteIteration(startDate, endDate);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteProject
        ///</summary>
        [TestMethod()]
        public void WriteProjectTest()
        {
            string projectName = string.Empty; // TODO: Initialize to an appropriate value
            Database.WriteProject(projectName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
