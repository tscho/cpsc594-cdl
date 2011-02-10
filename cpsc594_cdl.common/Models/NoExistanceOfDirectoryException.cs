using System;

namespace cpsc594_cdl.Common.Models
{
    [Serializable]
    public class NoExistanceOfDirectoryException : Exception
    {
        public string ErrorMessage
        {
            get{return base.Message.ToString();}
        }

        public NoExistanceOfDirectoryException(string errorMessage) : base(errorMessage) { }

        public NoExistanceOfDirectoryException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
    }
}
