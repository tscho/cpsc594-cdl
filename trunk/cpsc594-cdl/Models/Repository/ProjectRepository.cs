using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Util.Database;
using System.Configuration;

namespace cpsc594_cdl.Models.Repository
{
    public class ProjectRepository
    {
        //private SqlConnection connection;

        public ProjectRepository() {
            /*connection = new SqlConnection();
            var cs = ConfigurationManager.ConnectionStrings;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;*/
        }

        public List<Project> getProjects()
        {
            //var cmd = new StoredProcCommand("usp_GetProjects");
            //var results = cmd.ExecuteReader(connection, null);

            /*var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Projects";

            connection.Open();
            var results = cmd.ExecuteReader();

            var projects = new List<Project>();
            while (results.Read())
            {
                projects.Add(new Project(Convert.ToInt32(results["ID"]), results["Name"].ToString()));
            }

            connection.Close();

            return projects.ToArray();*/

            if(DatabaseAccessor.Connection())
            {
                List<Util.Database.Project> dbProjects = DatabaseAccessor.GetProjects();
                List<Project> projectList = new List<Project>();

                foreach (Util.Database.Project project in dbProjects)
                {
                    projectList.Add(new Project(project.ProjectID, project.ProjectName));
                }

                return projectList;
            }
            return null;
        }
    }
}