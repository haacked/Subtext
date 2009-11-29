#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Subtext.Scripting.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs during the execution of a script.
    /// </summary>
    /// <remarks>
    /// Contains a custom property, thus it Implements ISerializable 
    /// and the special serialization constructor.
    /// </remarks>
    [Serializable]
    public sealed class SqlScriptExecutionException : Exception, ISerializable
    {
        readonly int _returnValue;
        readonly Script _script;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
        /// </summary>
        public SqlScriptExecutionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SqlScriptExecutionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlScriptExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="script">The script.</param>
        /// <param name="returnValue">The return value.</param>
        public SqlScriptExecutionException(string message, Script script, int returnValue) : base(message)
        {
            _script = script;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="script">The script.</param>
        /// <param name="returnValue">The return value.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlScriptExecutionException(string message, Script script, int returnValue, Exception innerException)
            : base(message, innerException)
        {
            _script = script;
            _returnValue = returnValue;
        }

        // Because this class is sealed, this constructor is private. 
        // if this class is not sealed, this constructor should be protected.
        private SqlScriptExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _script = info.GetValue("Script", typeof(string)) as Script;
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        public Script Script
        {
            get { return _script; }
        }

        /// <summary>
        /// Gets the return value.
        /// </summary>
        /// <value>The return value.</value>
        public int ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                string message = base.Message;
                if(Script != null)
                {
                    message += string.Format(CultureInfo.InvariantCulture, "{0}ScriptName: {1}", Environment.NewLine,
                                             _script);
                }
                message += string.Format("Return Value: {0}", ReturnValue);
                return message;
            }
        }

        #region ISerializable Members

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>
        /// with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception 
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Script", _script);
            GetObjectData(info, context);
        }

        #endregion
    }
}