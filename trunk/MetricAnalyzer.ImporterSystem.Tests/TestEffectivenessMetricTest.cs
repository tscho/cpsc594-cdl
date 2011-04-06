using Importer_System.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Importer_System_tests
{
    
    
    /// <summary>
    ///This is a test class for TestEffectivenessMetricTest and is intended
    ///to contain all TestEffectivenessMetricTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestEffectivenessMetricTest
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
        ///A test for FindLineValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Importer_System.exe")]
        public void FindLineValueTest()
        {
            TestEffectivenessMetric_Accessor target = new TestEffectivenessMetric_Accessor(); // TODO: Initialize to an appropriate value
            string line = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.FindLineValue(line);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FindComponent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Importer_System.exe")]
        public void FindComponentTest()
        {
            TestEffectivenessMetric_Accessor target = new TestEffectivenessMetric_Accessor(); // TODO: Initialize to an appropriate value
            string line = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.FindComponent(line);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

    }
}
