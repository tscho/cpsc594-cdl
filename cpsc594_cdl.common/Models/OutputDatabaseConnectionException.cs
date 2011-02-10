using System;

namespace cpsc594_cdl.Common.Models
{
    [Serializable]
    public class OutputDatabaseConnectionException : Exception
    {
        public string ErrorMessage
        {
            get{return base.Message.ToString();}
        }

        public OutputDatabaseConnectionException(string errorMessage) : base(errorMessage) { }

        public OutputDatabaseConnectionException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) {}
    }
}
