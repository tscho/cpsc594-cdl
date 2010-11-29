using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Util.Database;

namespace cpsc594_cdl.Models.Repository
{
    public class IterationRepository
    {

        public IterationRepository() {
            /*connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;*/
        }

        public List<Iteration> getIterationsForComponent(DateTime startDate)
        {
            

            List<Util.Database.Iteration> dbIterations = DatabaseAccessor.GetIterations(startDate);
            List<Iteration> iterationList = new List<Iteration>();

            foreach (Util.Database.Iteration iteration in dbIterations)
            {
                iterationList.Add(new Iteration(iteration.StartDate, iteration.EndDate, iteration.IterationID, null));
            }

            return iterationList;

        }
    }
}