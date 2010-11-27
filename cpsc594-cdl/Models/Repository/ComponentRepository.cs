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
            /*connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;*/
        }

        public List<Component> getComponentsForProject(int pid)
        {
            //var cmd = new StoredProcCommand("usp_GetComponents");
            //var results = cmd.ExecuteReader(connection, new SqlParameter[] { new SqlParameter("pid", pid) });

            /*var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Components WHERE PID=" + pid;

            connection.Open();
            var results = cmd.ExecuteReader();

            var components = new List<Component>();
            while (results.Read())
            {
                components.Add(new Component(int.Parse(results["ID"].ToString()), results["Name"].ToString()));
            }
            connection.Close();

            return components.ToArray();*/

            List<Util.Database.Component> dbComponents = DatabaseAccessor.GetComponents(pid);
            List<Component> componentList = new List<Component>();

            foreach (Util.Database.Component component in dbComponents)
            {
                componentList.Add(new Component(component.ComponentID, component.ComponentName));
            }

            return componentList;

        }

        public string getName(int cid)
        {
            return "NAME";
        }

        public List<double> getCodeCoverage(int pid)
        {
            List<double> c_data = new List<double>();
            for (int i = 0; i < 8; i++)
            {
                c_data.Add(i * .5);
            }
            return c_data;
        }

        public List<int> getSample()
        {
            List<int> c_data = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                c_data.Add(i);
            }
            return c_data;
        }
    }
}