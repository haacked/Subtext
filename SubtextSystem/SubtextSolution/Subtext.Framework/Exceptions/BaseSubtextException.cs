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
		/// Returns a resource key for the message.  This is used to 
		/// look up the message in the correct language within a 
		/// resource file (when we get around to I8N).
		/// </summary>
		/// <value></value>
		public abstract string MessageResourceKey {get;}
	}
}
