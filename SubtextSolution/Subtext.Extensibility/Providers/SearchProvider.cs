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
using System.Collections.Generic;
using System.Configuration.Provider;

namespace Subtext.Extensibility.Providers
{
	public abstract class SearchProvider : ProviderBase
	{
		private static SearchProvider provider = null;
		private static GenericProviderCollection<SearchProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<SearchProvider>("Search", out provider);

		/// <summary>
		/// Returns the currently configured SearchProvider.
		/// </summary>
		/// <returns></returns>
		public static SearchProvider Instance()
		{
			return provider;
		}

		/// <summary>
		/// Returns all the configured SearchProviders.
		/// </summary>
		public static GenericProviderCollection<SearchProvider> Providers
		{
			get
			{
				return providers;
			}
		}
		
		public abstract IList<SearchResult> Search(int blogId, string searchTerm);
	}
	
	public struct SearchResult
	{
		public SearchResult(string title, Uri url)
		{
			this.title = title;
			this.url = url;
		}
		
		public string Title
		{
			get { return this.title; }
		}

		string title;

		public Uri Url
		{
			get { return this.url; }
		}

		Uri url;
	}
}
