using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Subtext.Scripting.Exceptions
{
    [Serializable]
    public class SqlParseException : Exception
    {
        public SqlParseException()
        {
        }

        public SqlParseException(string message) : base(message)
        {
        }

        public SqlParseException(string message, Exception exception)
            : base(message, exception)
        {
        }

        protected SqlParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}