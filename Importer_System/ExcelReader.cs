using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Importer_System
{
    class ExcelReader
    {
        private string connectionString;
        public ExcelReader(string connectionString)
        {
            this.connectionString=connectionString;
        }

        /// <summary>
        ///     CheckConnection - throws an exception which is handled above in resourceUtilization if no connect is made
        /// </summary>
        public void CheckConnection()
        {
            System.Data.OleDb.OleDbConnection ExcelConnection = new System.Data.OleDb.OleDbConnection(connectionString);
            System.Data.OleDb.OleDbCommand ExcelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [Sheet1$]", ExcelConnection);
            ExcelConnection.Open();
            System.Data.OleDb.OleDbDataReader ExcelReader;
            ExcelReader = ExcelCommand.ExecuteReader();
            ExcelReader.Read();
            ExcelConnection.Close();
        }

        /// <summary>
        ///     SelectQuery - takes in a query and executes it, returns a list of string, each results it on a line with a comma delimeter
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<string> SelectQuery(string query)
        {
            System.Data.OleDb.OleDbConnection ExcelConnection = new System.Data.OleDb.OleDbConnection(connectionString);

            System.Data.OleDb.OleDbCommand ExcelCommand = new System.Data.OleDb.OleDbCommand(query, ExcelConnection);
            ExcelConnection.Open();
            System.Data.OleDb.OleDbDataReader ExcelReader;

            ExcelReader = ExcelCommand.ExecuteReader();
            List<string> data = new List<string>();
            while (ExcelReader.Read())
            {
                string tempData = "";
                for (int i = 0; i < ExcelReader.FieldCount; i++)
                {
                    tempData += ExcelReader.GetValue(i).ToString();
                    if (i != ExcelReader.FieldCount - 1)
                        tempData += ",";
                }
                data.Add(tempData);
            }
            ExcelConnection.Close();
            return data;
        }
    }
}
