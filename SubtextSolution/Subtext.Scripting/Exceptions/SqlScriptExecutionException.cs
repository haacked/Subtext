#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Subtext.Scripting.Properties;

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
		Script _script;
		int _returnValue;

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
		public SqlScriptExecutionException(string message, Script script, int returnValue, Exception innerException) : base(message, innerException)
		{
			_script = script;
			_returnValue = returnValue;
		}

		// Because this class is sealed, this constructor is private. 
		// if this class is not sealed, this constructor should be protected.
		private SqlScriptExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			_script = info.GetValue("Script", typeof(Script)) as Script;
			_returnValue = (int)(info.GetValue("ReturnValue", typeof(int)) ?? 0);
		}

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>
		/// with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Script", _script);
			info.AddValue("ReturnValue", _returnValue);
			GetObjectData(info, context);
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
			get { return _returnValue;}
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
                if (this.Script != null)
                {
                    message += Environment.NewLine + Resources.SqlScriptExecution_ScriptName + _script;
                }
				message += Resources.SqlScriptExecution_ReturnValue + ReturnValue;
				return message;
			}
		}
	}
}
