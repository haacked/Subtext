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

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Custom collection of configured <see cref="ProviderInfo"/> 
	/// instances.
	/// </summary>
	[Serializable]
	public class ProviderCollection : System.Collections.Generic.Dictionary<String, ProviderInfo>
	{
		ProviderInfo _defaultProvider = null;

	    public void Add(ProviderInfo provider)
	    {
	        Add(provider.Name, provider);
	    }
	    
		/// <summary>
		/// Gets or sets the default provider.
		/// </summary>
		/// <value></value>
		public ProviderInfo DefaultProvider
		{
			get { return _defaultProvider; }
			set { _defaultProvider = value; }
		}
	}
}
