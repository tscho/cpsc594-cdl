using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Util.Database;
using System.Configuration;

namespace cpsc594_cdl.Models.Repository
{
    public class ComponentRepository
    {
        private SqlConnection connection;

        public ComponentRepository() {
            connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public Component[] getComponentsForProject(int pid) {
            //var cmd = new StoredProcCommand("usp_GetComponents");
            //var results = cmd.ExecuteReader(connection, new SqlParameter[] { new SqlParameter("pid", pid) });

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Components WHERE PID=" + pid;

            connection.Open();
            var results = cmd.ExecuteReader();

            var components = new List<Component>();
            while (results.Read())
            {
                components.Add(new Component(int.Parse(results["ID"].ToString()), results["Name"].ToString()));
            }
            connection.Close();

            return components.ToArray();
        }
    }
}