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
using Subtext.Extensibility.Providers;

namespace Subtext.Extensibility.Exceptions
{
	/// <summary>
	/// Exception thrown when unable to load a provider.
	/// </summary>
	[Serializable]
	public class ProviderInstantiationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProviderInstantiationException"/> class.
		/// </summary>
		public ProviderInstantiationException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProviderInstantiationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ProviderInstantiationException(string message) : base(message)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ProviderInstantiationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ProviderInstantiationException(string message, ProviderInfo providerInfo, Exception innerException) : this(message, providerInfo, string.Empty, innerException)
		{
			
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ProviderInstantiationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="providerInfo">Type of the provider.</param>
		/// <param name="sectionName">Name of the section.</param>
		/// <param name="innerException">The inner exception.</param>
		public ProviderInstantiationException(string message, ProviderInfo providerInfo, string sectionName, Exception innerException) : base(message, innerException)
		{
			this.providerInfo = providerInfo;
			this.sectionName = sectionName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProviderInstantiationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public ProviderInstantiationException(string message, Exception innerException) : base(message, innerException)
		{
		}
		
		/// <summary>
		/// Gets the type of the provider.
		/// </summary>
		/// <value>The type of the provider.</value>
		public ProviderInfo ProviderInfo
		{
			get
			{
				return this.providerInfo;
			}
		}
		
		ProviderInfo providerInfo;
		
		public string SectionName
		{
			get
			{
				return this.sectionName;
			}
		}
		
		string sectionName;
	}
}
