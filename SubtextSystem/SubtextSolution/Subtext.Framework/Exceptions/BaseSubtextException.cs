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
		protected BaseSubtextException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSubtextException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		protected BaseSubtextException(string message) : base(message)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSubtextException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		protected BaseSubtextException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Creates a new <see cref="BaseSubtextException"/> instance.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		protected BaseSubtextException(Exception innerException) : base(string.Empty, innerException)
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
