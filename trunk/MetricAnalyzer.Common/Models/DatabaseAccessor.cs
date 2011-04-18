using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MetricAnalyzer.Common.Models
{
    public static class DatabaseAccessor
    {
        private static MetricAnalyzerEntitiesDataContext _context;

        static DatabaseAccessor()
        {
            _context = new MetricAnalyzerEntitiesDataContext();
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
        public static Coverage GetCoverage(int iterationId, int componentId)
        {
            var componentCoverage = (from m in _context.Coverages where m.ComponentID == componentId && m.IterationID == iterationId orderby m.Date ascending select m).FirstOrDefault();

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
                    _context.Coverages.InsertOnSubmit(coverage);
                    _context.SubmitChanges();
                    id = coverage.CoverageID;
                }
                else{
                    entryExists.LinesCovered = linesCovered;
                    entryExists.LinesExecuted = linesExecuted;
                    entryExists.FileName = fileName;
                    _context.SubmitChanges();
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
                    _context.DefectInjectionRates.InsertOnSubmit(defectInjectionRate);
                    _context.SubmitChanges();
                    id = defectInjectionRate.DefectInjectionRateID;
                }
                else
                {
                    entryExists.NumberOfHighDefects = numberOfHighDefects;
                    entryExists.NumberOfMediumDefects = numberOfMediumDefects;
                    entryExists.NumberOfLowDefects = numberOfLowDefects;
                    _context.SubmitChanges();
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
                    _context.DefectRepairRates.InsertOnSubmit(defectRepairRate);
                    _context.SubmitChanges();
                    id = defectRepairRate.DefectRepairRateID;
                }
                else
                {
                    entryExists.NumberOfVerifiedDefects = numberOfVerifiedDefects;
                    entryExists.NumberOfResolvedDefects = numberOfResolvedDefects;
                    _context.SubmitChanges();
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
                    _context.TestEffectivenesses.InsertOnSubmit(testEffect);
                    _context.SubmitChanges();
                    id = testEffect.TestEffectivenessID;
                }
                else
                {
                    entryExists.TestCases = testCases;
                    _context.SubmitChanges();
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
        /// <param name="workActionId"></param>
        /// <param name="iterationId"></param>
        public static int WriteResourceUtilization(string productName, double hours, int workActionId, int iterationId)
        {
            var productResourceUtilization = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productResourceUtilization != null)
            {
                var entryExists = (from resource in _context.ResourceUtilizations where resource.ProductID == productResourceUtilization.ProductID && resource.IterationID == iterationId && resource.WorkActionID == workActionId select resource).FirstOrDefault();

                if (entryExists == null)
                {
                    var resourseUtil = new ResourceUtilization()
                    {
                        ProductID = productResourceUtilization.ProductID,
                        IterationID = iterationId,
                        WorkActionID = workActionId,
                        PersonHours = hours,
                        Date = DateTime.Now
                    };
                    _context.ResourceUtilizations.InsertOnSubmit(resourseUtil);
                    _context.SubmitChanges();
                    id = resourseUtil.ResourceUtilizationID;
                }
                else
                {
                    entryExists.PersonHours = hours;
                    _context.SubmitChanges();
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
                    _context.OutOfScopeWorks.InsertOnSubmit(outOfScopeWork);
                    _context.SubmitChanges();
                    id = outOfScopeWork.OutOfScopeWorkID;
                }
                else
                {
                    entryExists.PersonHours = hours;
                    _context.SubmitChanges();
                    id = entryExists.OutOfScopeWorkID;
                }
            }
            return id;
        }

        public static bool CheckForRework(string productName, int iterationId, int workActionId)
        {
            var product = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            if (product != null)
            {
                var entryExists = (from resource in _context.ResourceUtilizations
                                   where
                                       resource.ProductID == product.ProductID && resource.IterationID < iterationId &&
                                       resource.WorkActionID == workActionId
                                   select resource).FirstOrDefault();

                if (entryExists != null)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
            
        }

        public static int WriteReworkMetric(string productName, double reworkHours, int iterationId)
        {
            var productRework = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();

            int id = -1;

            if (productRework != null)
            {
                var entryExists = (from rework in _context.Reworks where rework.ProductID == productRework.ProductID && rework.IterationID == iterationId select rework).FirstOrDefault();

                if (entryExists == null)
                {
                    var rework = new Rework()
                    {
                        ProductID = productRework.ProductID,
                        IterationID = iterationId,
                        ReworkHours = reworkHours,
                        Date = DateTime.Now
                    };
                    _context.Reworks.InsertOnSubmit(rework);
                    _context.SubmitChanges();
                    id = rework.ReworkID;
                }
                else
                {
                    entryExists.ReworkHours = reworkHours;
                    //_context.AcceptAllChanges();
                    _context.SubmitChanges();
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
                    _context.VelocityTrends.InsertOnSubmit(velocityTrend);
                    _context.SubmitChanges();
                    id = velocityTrend.VelocityTrendID;
                }
                else
                {
                    entryExists.ActualHours = actualHours;
                    entryExists.EstimatedHours = estimatedHours;
                    _context.SubmitChanges();
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
            IOrderedQueryable<Product> products = (from p in _context.Products where pids.Contains(p.ProductID) orderby p.ProductName ascending select p);

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

            _context.Products.InsertOnSubmit(product);

            _context.SubmitChanges();
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
        public static Iteration WriteIteration(DateTime startDate, DateTime endDate, string label)
        {
            var iteration = new Iteration();
            iteration.StartDate = startDate.Date;
            iteration.EndDate = endDate.Date;
            iteration.IterationLabel = label;

            _context.Iterations.InsertOnSubmit(iteration);
            try
            {
                _context.SubmitChanges();
                return iteration;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException.Message);
                return null;
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
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations orderby i.StartDate descending select i);

            if (iterations != null)
            {
                return iterations.Take(limits).ToList();
            }
            else
            {
                return null;
            }
        }

        public static List<Iteration> GetIterations(int startID, int endID)
        {
            DateTime startDate = (from i in _context.Iterations where i.IterationID == startID select i.StartDate).FirstOrDefault();
            DateTime endDate = (from i in _context.Iterations where i.IterationID == endID select i.StartDate).FirstOrDefault();

            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations where i.StartDate >= startDate && i.StartDate <= endDate orderby i.StartDate ascending select i);

            if (iterations != null)
            {
                return iterations.ToList();
            }

            return null;
        }

        public static List<Iteration> GetIterations(IEnumerable<int> iterationIDs)
        {
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations where iterationIDs.Contains(i.IterationID) orderby i.IterationID ascending select i);

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

                _context.Components.InsertOnSubmit(component);
                _context.SubmitChanges();
            }
        }
    }
}
