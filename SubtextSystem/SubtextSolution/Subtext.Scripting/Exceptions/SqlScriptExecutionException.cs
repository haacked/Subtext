using System;
using System.Runtime.Serialization;

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
		Script _script = null;
		int _returnValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlScriptExecutionException"/> class.
		/// </summary>
		public SqlScriptExecutionException() : base()
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
			_script = info.GetValue("Script", typeof (string)) as Script;
		}

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>
		/// with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception 
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Script", _script);
			base.GetObjectData(info, context);
		}

		/// <summary>
		/// Gets the script.
		/// </summary>
		public Script scriptName
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
				if (scriptName != null)
					message += Environment.NewLine + "scriptName: " + _script.ToString();
				return message;
			}
		}
	}
}
