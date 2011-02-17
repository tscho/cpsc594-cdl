using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Importer_System
{
    [Serializable]
    public sealed class TerminateException : Exception
    {
        private string stringInfo;
        private bool booleanInfo;
        public TerminateException() : base() { }
        public TerminateException(string message) : base(message) { }
        public TerminateException(string message, Exception inner) : base(message, inner) { }
        private TerminateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            stringInfo = info.GetString("StringInfo");
            booleanInfo = info.GetBoolean("BooleanInfo");
        }

        public TerminateException(string message, string stringInfo, bool booleanInfo) : this(message)
        {
            this.stringInfo = stringInfo;
            this.booleanInfo = booleanInfo;
        }

        public TerminateException(string message, Exception inner, string stringInfo, bool booleanInfo)
            : this(message, inner)
        {
            this.stringInfo = stringInfo;
            this.booleanInfo = booleanInfo;
        }

        public string StringInfo
        {
            get { return stringInfo; }
        }

        public bool BooleanInfo
        {
            get { return booleanInfo; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StringInfo", stringInfo);
            info.AddValue("BooleanInfo", booleanInfo);

            base.GetObjectData(info, context);
        }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (stringInfo != null)
                {
                    message += Environment.NewLine + stringInfo + " = " + booleanInfo;
                }
                return message;
            }
        }
    }
}
