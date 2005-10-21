using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Base exception for subtext exceptions.
	/// </summary>
	[Serializable]
	public abstract class BaseSubtextException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="BaseSubtextException"/> instance.
		/// </summary>
		public BaseSubtextException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSubtextException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public BaseSubtextException(string message) : base(message)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSubtextException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public BaseSubtextException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Creates a new <see cref="BaseSubtextException"/> instance.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		public BaseSubtextException(Exception innerException) : base(string.Empty, innerException)
		{}

		/// <summary>
		/// Returns a resource key for the message.  This is used to 
		/// look up the message in the correct language within a 
		/// resource file (when we get around to I8N).
		/// </summary>
		/// <value></value>
		public abstract string MessageResourceKey {get;}
	}
}
