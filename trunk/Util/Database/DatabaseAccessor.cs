using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Util.Database
{
    public static class DatabaseAccessor
    {
        private static CPSC594Entities1 _context;

        static DatabaseAccessor()
        {
            _context = new CPSC594Entities1();
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
            if (_context.DatabaseExists())
                return true;
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
        /// <param name="projectName"></param>
        /// <param name="componentName"></param>
        /// <param name="linesCovered"></param>
        /// <param name="linesExecuted"></param>
        /// <param name="currIteration"></param>
        /// <returns></returns>
        public static int WriteCodeCoverage(string projectName, string componentName, int linesCovered, int linesExecuted, int currIteration)
        {
            var componentCoverage = (from p in _context.Projects join c in _context.Components on p.ProjectID equals c.ProjectID where p.ProjectName == projectName && c.ComponentName == componentName select c).FirstOrDefault();

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
        public static void WriteIteration(DateTime startDate, DateTime endDate)
        {
            var iteration = new Iteration();
            iteration.StartDate = startDate.Date;
            iteration.EndDate = endDate.Date;

            _context.Iterations.AddObject(iteration);
            _context.SaveChanges();
        }

        public static Iteration GetLastIteration()
        {
            var lastIteration =
                (from i in _context.Iterations
                 orderby i.IterationID ascending
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
            IOrderedQueryable<Iteration> iterations = (from i in _context.Iterations orderby i.IterationID descending select i);
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
        public static List<Component> GetComponents(int projectId)
        {
            IOrderedQueryable<Component> components;

            components = (from c in _context.Components where c.ProjectID == projectId orderby c.ComponentName ascending select c);

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
        /// <param name="projectName"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static bool ComponentExists(string projectName, string componentName)
        {
            var componenet = (from p in _context.Projects join c in _context.Components on p.ProjectID equals c.ProjectID where p.ProjectName == projectName && c.ComponentName == componentName select c).FirstOrDefault();

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
        /// <param name="projectName"></param>
        /// <param name="componenetName"></param>
        public static void WriteComponent(string projectName, string componenetName)
        {
            var componentProject = (from c in _context.Projects where c.ProjectName == projectName select c).FirstOrDefault();
            if (componentProject != null)
            {
                var component = new Component { ProjectID = componentProject.ProjectID, ComponentName = componenetName };

                _context.Components.AddObject(component);
                _context.SaveChanges();
            }
        }

        //Project database methods
        public static Project GetProject(int pid)
        {
            Project project = (from p in _context.Projects where p.ProjectID == pid select p).FirstOrDefault();
            return project;
        }

        public static List<Project> GetProjects()
        {
            IOrderedQueryable<Project> projects = (from p in _context.Projects orderby p.ProjectName ascending select p);

            if(projects != null)
            {
                return projects.ToList();
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        public static void WriteProject(string projectName)
        {
            var project = new Project();

            project.ProjectName = projectName;

            _context.Projects.AddObject(project);

            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static bool ProjectExists(string projectName)
        {
            var project = (from c in _context.Projects where c.ProjectName == projectName select c).FirstOrDefault();

            if (project != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
