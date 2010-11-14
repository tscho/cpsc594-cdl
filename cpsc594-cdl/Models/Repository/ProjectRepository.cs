using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Util.Database;

namespace cpsc594_cdl.Models.Repository
{
    public class ProjectRepository
    {
        private SqlConnection connection;

        public ProjectRepository() {
            connection = new SqlConnection();
        }

        public Project[] getProjects() {
            var cmd = new StoredProcCommand("usp_GetProjects");
            var results = cmd.ExecuteReader(connection, null);

            var projects = new Stack<Project>();
            while (results.Read())
            {
                //map
            }

            return projects.ToArray();
        }
    }
}