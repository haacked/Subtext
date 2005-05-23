using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Base exception class for blog configuration errors.
	/// </summary>
	public abstract class BaseBlogConfigurationException : BaseSubtextException
	{	
		/// <summary>
		/// Creates a new <see cref="BaseBlogConfigurationException"/> instance.
		/// </summary>
		public BaseBlogConfigurationException() : base()
		{}

		/// <summary>
		/// Creates a new <see cref="BaseBlogConfigurationException"/> instance.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		public BaseBlogConfigurationException(Exception innerException) : base(innerException)
		{}
	}
}
