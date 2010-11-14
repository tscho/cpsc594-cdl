using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Util.Database;

namespace cpsc594_cdl.Models.Repository
{
    public class ComponentRepository
    {
        private SqlConnection connection;

        public ComponentRepository() {
            connection = new SqlConnection();
        }

        public Component[] getComponentsForProduct(int pid) {
            var cmd = new StoredProcCommand("usp_GetComponents");
            var results = cmd.ExecuteReader(connection, new SqlParameter[] { new SqlParameter("pid", pid) });

            var components = new Stack<Component>();
            while (results.Read())
            {
                //map
            }

            return components.ToArray();
        }
    }
}