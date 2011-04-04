using cpsc594_cdl.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Importer_System_tests
{
    
    
    /// <summary>
    ///This is a test class for DatabaseAccessorTest and is intended
    ///to contain all DatabaseAccessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabaseAccessorTest
    {


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
        ///A test for GetCoverage
        ///</summary>
        [TestMethod()]
        public void GetCoverageTest()
        {
            int iterationID = 0; // TODO: Initialize to an appropriate value
            int componentID = 0; // TODO: Initialize to an appropriate value
            Coverage expected = null; // TODO: Initialize to an appropriate value
            Coverage actual;
            actual = DatabaseAccessor.GetCoverage(iterationID, componentID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDefectInjectionRates
        ///</summary>
        [TestMethod()]
        public void GetDefectInjectionRatesTest()
        {
            int iterationID = 0; // TODO: Initialize to an appropriate value
            int componentID = 0; // TODO: Initialize to an appropriate value
            DefectInjectionRate expected = null; // TODO: Initialize to an appropriate value
            DefectInjectionRate actual;
            actual = DatabaseAccessor.GetDefectInjectionRates(iterationID, componentID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetIterations
        ///</summary>
        [TestMethod()]
        public void GetIterationsTest()
        {
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            List<Iteration> expected = null; // TODO: Initialize to an appropriate value
            List<Iteration> actual;
            actual = DatabaseAccessor.GetIterations(startDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetIterations
        ///</summary>
        [TestMethod()]
        public void GetIterationsTest1()
        {
            int startId = 0; // TODO: Initialize to an appropriate value
            int endId = 0; // TODO: Initialize to an appropriate value
            List<Iteration> expected = null; // TODO: Initialize to an appropriate value
            List<Iteration> actual;
            actual = DatabaseAccessor.GetIterations(startId, endId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetIterations
        ///</summary>
        [TestMethod()]
        public void GetIterationsTest2()
        {
            int limits = 0; // TODO: Initialize to an appropriate value
            List<Iteration> expected = null; // TODO: Initialize to an appropriate value
            List<Iteration> actual;
            actual = DatabaseAccessor.GetIterations(limits);
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
            actual = DatabaseAccessor.GetLastIteration();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProduct
        ///</summary>
        [TestMethod()]
        public void GetProductTest()
        {
            int pid = 0; // TODO: Initialize to an appropriate value
            Product expected = null; // TODO: Initialize to an appropriate value
            Product actual;
            actual = DatabaseAccessor.GetProduct(pid);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProducts
        ///</summary>
        [TestMethod()]
        public void GetProductsTest()
        {
            List<Product> expected = null; // TODO: Initialize to an appropriate value
            List<Product> actual;
            actual = DatabaseAccessor.GetProducts();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProducts
        ///</summary>
        [TestMethod()]
        public void GetProductsTest1()
        {
            IEnumerable<int> pids = null; // TODO: Initialize to an appropriate value
            IEnumerable<Product> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Product> actual;
            actual = DatabaseAccessor.GetProducts(pids);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ProductExists
        ///</summary>
        [TestMethod()]
        public void ProductExistsTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = DatabaseAccessor.ProductExists(productName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteCodeCoverage
        ///</summary>
        [TestMethod()]
        public void WriteCodeCoverageTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            int linesCovered = 0; // TODO: Initialize to an appropriate value
            int linesExecuted = 0; // TODO: Initialize to an appropriate value
            int currIteration = 0; // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteCodeCoverage(productName, componentName, linesCovered, linesExecuted, currIteration, fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteComponent
        ///</summary>
        [TestMethod()]
        public void WriteComponentTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            string componenetName = string.Empty; // TODO: Initialize to an appropriate value
            DatabaseAccessor.WriteComponent(productName, componenetName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteDefectInjectionRate
        ///</summary>
        [TestMethod()]
        public void WriteDefectInjectionRateTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            int numberOfHighDefects = 0; // TODO: Initialize to an appropriate value
            int numberOfMediumDefects = 0; // TODO: Initialize to an appropriate value
            int numberOfLowDefects = 0; // TODO: Initialize to an appropriate value
            int currIteration = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteDefectInjectionRate(productName, componentName, numberOfHighDefects, numberOfMediumDefects, numberOfLowDefects, currIteration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteDefectRepairRate
        ///</summary>
        [TestMethod()]
        public void WriteDefectRepairRateTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            int numberOfVerifiedDefects = 0; // TODO: Initialize to an appropriate value
            int numberOfResolvedDefects = 0; // TODO: Initialize to an appropriate value
            int currIteration = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteDefectRepairRate(productName, componentName, numberOfVerifiedDefects, numberOfResolvedDefects, currIteration);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteIteration
        ///</summary>
        [TestMethod()]
        public void WriteIterationTest()
        {
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            string label = string.Empty; // TODO: Initialize to an appropriate value
            DatabaseAccessor.WriteIteration(startDate, endDate, label);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteOutOfScopeWork
        ///</summary>
        [TestMethod()]
        public void WriteOutOfScopeWorkTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            double hours = 0F; // TODO: Initialize to an appropriate value
            int iterationID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteOutOfScopeWork(productName, hours, iterationID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteProduct
        ///</summary>
        [TestMethod()]
        public void WriteProductTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            DatabaseAccessor.WriteProduct(productName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteResourceUtilization
        ///</summary>
        [TestMethod()]
        public void WriteResourceUtilizationTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            double hours = 0F; // TODO: Initialize to an appropriate value
            int workActionId = 0; // TODO: Initialize to an appropriate value
            int iterationId = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteResourceUtilization(productName, hours, workActionId, iterationId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteReworkMetric
        ///</summary>
        [TestMethod()]
        public void WriteReworkMetricTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            double reworkHours = 0F; // TODO: Initialize to an appropriate value
            int iterationId = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteReworkMetric(productName, reworkHours, iterationId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteTestEffectiveness
        ///</summary>
        [TestMethod()]
        public void WriteTestEffectivenessTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            int testCases = 0; // TODO: Initialize to an appropriate value
            int iterationID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteTestEffectiveness(productName, testCases, iterationID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteVelocityTrendMetric
        ///</summary>
        [TestMethod()]
        public void WriteVelocityTrendMetricTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            double estimatedHours = 0F; // TODO: Initialize to an appropriate value
            double actualHours = 0F; // TODO: Initialize to an appropriate value
            int iterationID = 0; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = DatabaseAccessor.WriteVelocityTrendMetric(productName, estimatedHours, actualHours, iterationID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetComponents
        ///</summary>
        [TestMethod()]
        public void GetComponentsTest()
        {
            int productId = 0; // TODO: Initialize to an appropriate value
            List<Component> expected = null; // TODO: Initialize to an appropriate value
            List<Component> actual;
            actual = DatabaseAccessor.GetComponents(productId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetComponents
        ///</summary>
        [TestMethod()]
        public void GetComponentsTest1()
        {
            IEnumerable<int> ComponentIDs = null; // TODO: Initialize to an appropriate value
            List<Component> expected = null; // TODO: Initialize to an appropriate value
            List<Component> actual;
            actual = DatabaseAccessor.GetComponents(ComponentIDs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ComponentExists
        ///</summary>
        [TestMethod()]
        public void ComponentExistsTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            string componentName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = DatabaseAccessor.ComponentExists(productName, componentName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CheckForRework
        ///</summary>
        [TestMethod()]
        public void CheckForReworkTest()
        {
            string productName = string.Empty; // TODO: Initialize to an appropriate value
            int iterationId = 0; // TODO: Initialize to an appropriate value
            int workActionId = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = DatabaseAccessor.CheckForRework(productName, iterationId, workActionId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
