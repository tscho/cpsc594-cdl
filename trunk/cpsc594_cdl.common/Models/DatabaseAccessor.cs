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
        /// Writes code coverage values to the Database. Updates them if an entry already exists for that component and iteration.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="linesCovered"></param>
        /// <param name="linesExecuted"></param>
        /// <param name="currIteration"></param>
        /// <returns></returns>
        public static int WriteCodeCoverage(string productName, string componentName, int linesCovered, int linesExecuted, int currIteration, string fileName)
        {
            var componentCoverage = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();
            int id = -1;

            if (componentCoverage != null)
            {
                var entryExists = (from coverage in _context.Coverages where coverage.ComponentID == componentCoverage.ComponentID && coverage.IterationID == currIteration select coverage).FirstOrDefault();
                if(entryExists == null)
                {
                    var coverage = new Coverage
                    {
                        ComponentID = componentCoverage.ComponentID,
                        IterationID = currIteration,
                        LinesCovered = linesCovered,
                        LinesExecuted = linesExecuted,
                        FileName = fileName
                    };
                    _context.Coverages.AddObject(coverage);
                    _context.SaveChanges();
                    id = coverage.CoverageID;
                }
                else{
                    entryExists.LinesCovered = linesCovered;
                    entryExists.LinesExecuted = linesExecuted;
                    entryExists.FileName = fileName;
                    _context.SaveChanges();
                    id = entryExists.CoverageID;
                }
            }
            return id;
        }

        /// <summary>
        /// Writes defect injection rate values to the Database. Updates them if an entry already exists for that component and iteration.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="componentName"></param>
        /// <param name="numberOfHighDefects"></param>
        /// <param name="numberOfMediumDefects"></param>
        /// <param name="numberOfLowDefects"></param>
        public static int WriteDefectInjectionRate(string productName, string componentName, int numberOfHighDefects, int numberOfMediumDefects, int numberOfLowDefects, int currIteration)
        {
            var componentDefectInjectionRate = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();
            int id = -1;
            if (componentDefectInjectionRate != null)
            {
                var entryExists = (from defectInjection in _context.DefectInjectionRates where defectInjection.ComponentID == componentDefectInjectionRate.ComponentID && defectInjection.IterationID == currIteration select defectInjection).FirstOrDefault();
                if (entryExists == null)
                {

                    var defectInjectionRate = new DefectInjectionRate
                    {
                        ComponentID = componentDefectInjectionRate.ComponentID,
                        NumberOfHighDefects = numberOfHighDefects,
                        IterationID = currIteration,
                        NumberOfMediumDefects = numberOfMediumDefects,
                        NumberOfLowDefects = numberOfLowDefects,
                        Date = DateTime.Now
                    };
                    _context.DefectInjectionRates.AddObject(defectInjectionRate);
                    _context.SaveChanges();
                    id = defectInjectionRate.DefectInjectionRateID;
                }
                else
                {
                    entryExists.NumberOfHighDefects = numberOfHighDefects;
                    entryExists.NumberOfMediumDefects = numberOfMediumDefects;
                    entryExists.NumberOfLowDefects = numberOfLowDefects;
                    _context.SaveChanges();
                    id = entryExists.DefectInjectionRateID;
                }
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
        public static int WriteDefectRepairRate(string productName, string componentName, int numberOfVerifiedDefects, int numberOfResolvedDefects, int currIteration)
        {
            var componentDefectRepairRate = (from p in _context.Products join c in _context.Components on p.ProductID equals c.ProductID where p.ProductName == productName && c.ComponentName == componentName select c).FirstOrDefault();
            int id = -1;
            if (componentDefectRepairRate != null)
            {
                var entryExists = (from defectRepair in _context.DefectRepairRates where defectRepair.ComponentID == componentDefectRepairRate.ComponentID && defectRepair.IterationID == currIteration select defectRepair).FirstOrDefault();
                if (entryExists == null)
                {

                    var defectRepairRate = new DefectRepairRate
                    {
                        ComponentID = componentDefectRepairRate.ComponentID,
                        NumberOfVerifiedDefects = numberOfVerifiedDefects,
                        NumberOfResolvedDefects = numberOfResolvedDefects,
                        IterationID = currIteration,
                        Date = DateTime.Now
                    };
                    _context.DefectRepairRates.AddObject(defectRepairRate);
                    _context.SaveChanges();
                    id = defectRepairRate.DefectRepairRateID;
                }
                else
                {
                    entryExists.NumberOfVerifiedDefects = numberOfVerifiedDefects;
                    entryExists.NumberOfResolvedDefects = numberOfResolvedDefects;
                    _context.SaveChanges();
                    id = entryExists.DefectRepairRateID;
                }
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
            var productTestCases = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productTestCases != null)
            {
                var entryExists = (from testEffect in _context.TestEffectivenesses where testEffect.ProductID == productTestCases.ProductID && testEffect.IterationID == iterationID select testEffect).FirstOrDefault();

                if (entryExists == null)
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
                else
                {
                    entryExists.TestCases = testCases;
                    _context.SaveChanges();
                    id = entryExists.TestEffectivenessID;
                }
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
        public static int WriteResourceUtilization(string productName, double hours, int iterationID)
        {
            var productResourceUtilization = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productResourceUtilization != null)
            {
                var entryExists = (from resource in _context.ResourceUtilizations where resource.ProductID == productResourceUtilization.ProductID && resource.IterationID == iterationID select resource).FirstOrDefault();

                if (entryExists == null)
                {
                    var resourseUtil = new ResourceUtilization()
                    {
                        ProductID = productResourceUtilization.ProductID,
                        IterationID = iterationID,
                        PersonHours = hours,
                        Date = DateTime.Now
                    };
                    _context.ResourceUtilizations.AddObject(resourseUtil);
                    _context.SaveChanges();
                    id = resourseUtil.ResourceUtilizationID;
                }
                else
                {
                    entryExists.PersonHours = hours;
                    _context.SaveChanges();
                    id = entryExists.ResourceUtilizationID;
                }
            }
            return id;
        }

        public static int WriteOutOfScopeWork(string productName, double hours, int iterationID)
        {
            var productOutOfScopeWork = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productOutOfScopeWork != null)
            {
                var entryExists = (from scope in _context.OutOfScopeWorks where scope.ProductID == productOutOfScopeWork.ProductID && scope.IterationID == iterationID select scope).FirstOrDefault();

                if (entryExists == null)
                {
                    var outOfScopeWork = new OutOfScopeWork()
                    {
                        ProductID = productOutOfScopeWork.ProductID,
                        IterationID = iterationID,
                        PersonHours = hours,
                        Date = DateTime.Now
                    };
                    _context.OutOfScopeWorks.AddObject(outOfScopeWork);
                    _context.SaveChanges();
                    id = outOfScopeWork.OutOfScopeWorkID;
                }
                else
                {
                    entryExists.PersonHours = hours;
                    _context.SaveChanges();
                    id = entryExists.OutOfScopeWorkID;
                }
            }
            return id;
        }

        public static int WriteReworkMetric(string productName, double reworkHours, int iterationID)
        {
            var productRework = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productRework != null)
            {
                var entryExists = (from rework in _context.Reworks where rework.ProductID == productRework.ProductID && rework.IterationID == iterationID select rework).FirstOrDefault();

                if (entryExists == null)
                {
                    var rework = new Rework()
                    {
                        ProductID = productRework.ProductID,
                        IterationID = iterationID,
                        ReworkHours = reworkHours,
                        Date = DateTime.Now
                    };
                    _context.Reworks.AddObject(rework);
                    _context.SaveChanges();
                    id = rework.ReworkID;
                }
                else
                {
                    entryExists.ReworkHours = reworkHours;
                    _context.SaveChanges();
                    id = entryExists.ReworkID;
                }
            }
            return id;
        }

        public static int WriteVelocityTrendMetric(string productName,  double estimatedHours, double actualHours, int iterationID)
        {
            var productVelocityTrend = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productVelocityTrend != null)
            {
                var entryExists = (from velocity in _context.VelocityTrends where velocity.ProductID == productVelocityTrend.ProductID && velocity.IterationID == iterationID select velocity).FirstOrDefault();

                if (entryExists == null)
                {
                    var velocityTrend = new VelocityTrend()
                    {
                        ProductID = productVelocityTrend.ProductID,
                        IterationID = iterationID,
                        EstimatedHours = estimatedHours,
                        ActualHours = actualHours,
                        Date = DateTime.Now
                    };
                    _context.VelocityTrends.AddObject(velocityTrend);
                    _context.SaveChanges();
                    id = velocityTrend.VelocityTrendID;
                }
                else
                {
                    entryExists.ActualHours = actualHours;
                    entryExists.EstimatedHours = estimatedHours;
                    _context.SaveChanges();
                    id = entryExists.VelocityTrendID;
                }
            }
            return id;
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

            if (products != null)
            {
                return products.ToList();
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Product> GetProducts(IEnumerable<int> pids)
        {
            IOrderedQueryable<Product> products = (from p in _context.Products where pids.Contains(p.ProductID) orderby p.ProductName ascending select p );

            return products;
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
            iterations.Take(limits);

            if (iterations != null)
            {
                return iterations.ToList();
            }
            else
            {
                return null;
            }
        }

        public static List<Iteration> GetIterations(int startId, int endId)
        {
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations where i.IterationID >= startId && i.IterationID <= endId orderby i.IterationID ascending select i);

            if (iterations != null)
            {
                return iterations.ToList();
            }

            return null;
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
    }
}
