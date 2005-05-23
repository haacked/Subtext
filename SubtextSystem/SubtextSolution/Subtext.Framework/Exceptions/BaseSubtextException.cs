using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Summary description for BaseSubtextException.
	/// </summary>
	public abstract class BaseSubtextException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="BaseSubtextException"/> instance.
		/// </summary>
		public BaseSubtextException() : base()
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
