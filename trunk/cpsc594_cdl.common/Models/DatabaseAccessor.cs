using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace cpsc594_cdl.Common.Models
{
    public static class DatabaseAccessor
    {
        private static CPSC594Entities _context;

        static DatabaseAccessor()
        {
            _context = new CPSC594Entities();
        }

        /// <summary>
        ///     Attempts to establish a connection with the given connection string.
        ///     If fails
        ///         return false
        ///     Else
        ///         return true
        /// </summary>
        /// <param name="connection">Database connection string.</param>
        /// <returns>true if no error occured, false otherwise</returns>
        public static Boolean Connection()
        {
            try
            {
                if (_context.DatabaseExists())
                    return true;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return false;
            }
            return false;
        }

        //Metric database methods
        public static Coverage GetCoverage(int iterationID, int componentID)
        {
            var componentCoverage = (from m in _context.Coverages where m.ComponentID == componentID && m.IterationID == iterationID orderby m.Date ascending select m).FirstOrDefault();

            if(componentCoverage != null)
            {
                return componentCoverage;
            }
            else
            {
                return null;
            }
        }

        public static DefectInjectionRate GetDefectInjectionRates(int iterationID, int componentID)
        {
            var componentDejectInjection = (from m in _context.DefectInjectionRates where m.ComponentID == componentID && m.IterationID == iterationID orderby m.Date ascending select m).FirstOrDefault();

            if (componentDejectInjection != null)
            {
                return componentDejectInjection;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="fileName"></param>
        public static void UpdateCoverage(int id, DateTime date, string fileName)
        {
            var key = new EntityKey("CPSC594Entities.Coverages", "CoverageID", id);

            var coverage = (Coverage)_context.GetObjectByKey(key);
            if (coverage != null)
            {
                coverage.Date = date;
                coverage.FileName = fileName;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="linesCovered"></param>
        /// <param name="linesExecuted"></param>
        /// <param name="currIteration"></param>
        /// <returns></returns>
        public static int WriteCodeCoverage(string productName, string componentName, int linesCovered, int linesExecuted, int currIteration)
        {
            var componentCoverage = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();

            int id = -1;

            if (componentCoverage != null)
            {
                var coverage = new Coverage
                {
                    ComponentID = componentCoverage.ComponentID,
                    IterationID = currIteration,
                    LinesCovered = linesCovered,
                    LinesExecuted = linesExecuted
                };
                _context.Coverages.AddObject(coverage);
                _context.SaveChanges();
                id = coverage.CoverageID;
            }
            return id;
        }

        //Iteration database methods
        public static void WriteIteration(DateTime startDate, DateTime endDate, string label)
        {
            var iteration = new Iteration();
            iteration.StartDate = startDate.Date;
            iteration.EndDate = endDate.Date;
            iteration.IterationLabel = label;

            _context.Iterations.AddObject(iteration);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException.Message);
            }
        }

        public static Iteration GetLastIteration()
        {
            var lastIteration =
                (from i in _context.Iterations
                 orderby i.IterationID descending
                 select i).FirstOrDefault();
            if (lastIteration != null)
            {
                return lastIteration;
            }
            else
            {
                return null;
            }
        }

        public static List<Iteration> GetIterations(DateTime startDate)
        {
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations where i.StartDate >= startDate orderby i.IterationID ascending select i);

            if (iterations != null)
            {
                return iterations.ToList();
            }
            else
            {
                return null;
            }
        }

        public static List<Iteration> GetIterations(int limits)
        {
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations orderby i.IterationID ascending select i);
            iterations.Take(12);

            if (iterations != null)
            {
                return iterations.ToList();
            }
            else
            {
                return null;
            }
        }

        //Component database methods
        public static List<Component> GetComponents(int productId)
        {
            IOrderedQueryable<Component> components;

            components = (from c in _context.Components where c.ProductID == productId orderby c.ComponentName ascending select c);

            if (components != null)
            {
                return components.ToList();
            }
            else
            {
                return null;
            }
        }

        public static List<Component> GetComponents(IEnumerable<int> ComponentIDs)
        {
            IOrderedQueryable<Component> components;

            components = (from c in _context.Components where ComponentIDs.Contains(c.ComponentID) orderby c.ComponentName ascending select c);

            if (components != null)
            {
                return components.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static bool ComponentExists(string productName, string componentName)
        {
            var componenet = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();

            if (componenet != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductName"></param>
        /// <param name="componenetName"></param>
        public static void WriteComponent(string productName, string componenetName)
        {
            var componentProduct = (from c in _context.Products where c.ProductName == productName select c).FirstOrDefault();
            if (componentProduct != null)
            {
                var component = new Component { ProductID = componentProduct.ProductID, ComponentName = componenetName };

                _context.Components.AddObject(component);
                _context.SaveChanges();
            }
        }

        //Product database methods
        public static Product GetProduct(int pid)
        {
            Product product = (from p in _context.Products where p.ProductID == pid select p).FirstOrDefault();
            return product;
        }

        public static List<Product> GetProducts()
        {
            IOrderedQueryable<Product> products = (from p in _context.Products orderby p.ProductName ascending select p);

            if(products != null)
            {
                return products.ToList();
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        public static void WriteProduct(string productName)
        {
            var product = new Product();

            product.ProductName = productName;

            _context.Products.AddObject(product);

            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static bool ProductExists(string productName)
        {
            var product = (from c in _context.Products where c.ProductName == productName select c).FirstOrDefault();

            if (product != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractID"></param>
        /// <returns></returns>
        public static bool ContractExists(int contractID)
        {
            var product = (from c in _context.Contracts where c.ContractID == contractID select c).FirstOrDefault();

            if (product != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractID"></param>
        public static void WriteContract(int contractID)
        {
            var contract = new Contract();

            contract.ContractID = contractID;

            _context.Contracts.AddObject(contract);

            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="numberOfHighDefects"></param>
        /// <param name="numberOfMediumDefects"></param>
        /// <param name="numberOfLowDefects"></param>
        public static int WriteDefectInjectionRate(string productName, string componentName, int numberOfHighDefects, int numberOfMediumDefects, int numberOfLowDefects, int curIteration)
        {
            var componentDefectInjectionRate = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();
            int id = -1;
            if (componentDefectInjectionRate != null)
            {
                var defectInjectionRate = new DefectInjectionRate
                {
                    ComponentID = componentDefectInjectionRate.ComponentID,
                    NumberOfHighDefects = numberOfHighDefects,
                    IterationID = curIteration,
                    NumberOfMediumDefects = numberOfMediumDefects,
                    NumberOfLowDefects = numberOfLowDefects,
                    Date = DateTime.Now
                };
                _context.DefectInjectionRates.AddObject(defectInjectionRate);
                try
                {
                    _context.SaveChanges();
                }
                catch
                {}
                id = defectInjectionRate.DefectInjectionRateID;
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="numberOfVerifiedDefects"></param>
        /// <param name="numberOfResolvedDefects"></param>
        /// <param name="curIteration"></param>
        /// <returns></returns>
        public static int WriteDefectRepairRate(string productName, string componentName, int numberOfVerifiedDefects, int numberOfResolvedDefects, int curIteration)
        {
            var componentDefectRepairRate = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();
            int id = -1;
            if (componentDefectRepairRate != null)
            {
                var defectRepairRate = new DefectRepairRate
                {
                    ComponentID = componentDefectRepairRate.ComponentID,
                    NumberOfVerifiedDefects = numberOfVerifiedDefects,
                    NumberOfResolvedDefects = numberOfResolvedDefects,
                    IterationID = curIteration,
                    Date = DateTime.Now
                };
                _context.DefectRepairRates.AddObject(defectRepairRate);
                _context.SaveChanges();
                id = defectRepairRate.DefectRepairRateID;
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="testCases"></param>
        /// <param name="iterationID"></param>
        /// <returns></returns>
        public static int WriteTestEffectiveness(string productName, int testCases, int iterationID)
        {
            var productTestCases =
                (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productTestCases != null)
            {
                var testEffect = new TestEffectiveness()
                {
                    ProductID = productTestCases.ProductID,
                    IterationID = iterationID,
                    TestCases = testCases,
                    Date = DateTime.Now
                };
                _context.TestEffectivenesses.AddObject(testEffect);
                _context.SaveChanges();
                id = testEffect.TestEffectivenessID;
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="personName"></param>
        /// <param name="hours"></param>
        /// <param name="p"></param>
        public static int WriteResourceUtilization(string productName, int contractID, double hours, int iterationID)
        {
            var productResourceUtilization = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productResourceUtilization != null)
            {
                var resourseUtil = new ResourceUtilization()
                {
                    ProductID = productResourceUtilization.ProductID,
                    IterationID = iterationID,
                    ContractID = contractID,
                    PersonHours = hours,
                    Date = DateTime.Now
                };
                _context.ResourceUtilizations.AddObject(resourseUtil);
                _context.SaveChanges();
                id = resourseUtil.ResourceUtilizationID;
            }
            return id;
        }

        public static int WriteOutOfScopeWork(string productName, int contractID, double hours, int iterationID)
        {
            var productOutOfScopeWork = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productOutOfScopeWork != null)
            {
                var outOfScopeWork = new OutOfScopeWork()
                {
                    ProductID = productOutOfScopeWork.ProductID,
                    IterationID = iterationID,
                    ContractID = contractID,
                    PersonHours = hours,
                    Date = DateTime.Now
                };
                _context.OutOfScopeWorks.AddObject(outOfScopeWork);
                _context.SaveChanges();
                id = outOfScopeWork.OutOfScopeWorkID;
            }
            return id;
        }

        public static int WriteReworkMetric(string productName, int contractID, double reworkHours, int iterationID)
        {
            var productRework = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productRework != null)
            {
                var rework = new Rework()
                {
                    ProductID = productRework.ProductID,
                    IterationID = iterationID,
                    ContractID = contractID,
                    ReworkHours = reworkHours,
                    Date = DateTime.Now
                };
                _context.Reworks.AddObject(rework);
                _context.SaveChanges();
                id = rework.ReworkID;
            }
            return id;
        }

        public static int WriteVelocityTrendMetric(string productName, int contractID, double estimatedHours, double actualHours, int iterationID)
        {
            var productVelocityTrend = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productVelocityTrend != null)
            {
                var velocityTrend = new VelocityTrend()
                {
                    ProductID = productVelocityTrend.ProductID,
                    IterationID = iterationID,
                    ContractID = contractID,
                    EstimatedHours = estimatedHours,
                    ActualHours = actualHours,
                    Date = DateTime.Now
                };
                _context.VelocityTrends.AddObject(velocityTrend);
                _context.SaveChanges();
                id = velocityTrend.VelocityTrendID;
            }
            return id;
        }
    }
}
