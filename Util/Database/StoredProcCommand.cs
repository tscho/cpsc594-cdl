using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Util.Database
{
    public class StoredProcCommand
    {
        private SqlCommand command;

        public StoredProcCommand(String commandName)
        {
            command = new SqlCommand("exec " + commandName);
        }

        public SqlDataReader ExecuteReader(SqlConnection connection, SqlParameter[] parameters)
        {
            command.Connection = connection;
            command.Parameters.AddRange(parameters);
            return command.ExecuteReader();
        }

        public void ExecuteNonReader(SqlConnection connection, SqlParameter[] parameters)
        {
            command.Connection = connection;
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }
    }
}