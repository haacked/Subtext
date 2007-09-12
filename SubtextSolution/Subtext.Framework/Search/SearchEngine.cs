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
using Subtext.Extensibility.Providers;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Search
{
	/// <summary>
	/// Starting point for the Search API.
	/// </summary>
	public static class SearchEngine
	{
		private static SearchProvider provider;
		private static GenericProviderCollection<SearchProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<SearchProvider>("Search", out provider);

		/// <summary>
		/// Returns the currently configured SearchProvider.
		/// </summary>
		/// <returns></returns>
		public static SearchProvider Provider
		{
			get 
			{
				return provider;
			}
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

		/// <summary>
		/// Searches the specified blog for items that match the search term.
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="searchTerm"></param>
		/// <returns></returns>
		public static IList<SearchResult> Search(int blogId, string searchTerm)
		{
			if (String.IsNullOrEmpty(searchTerm))
				throw new ArgumentNullException("searchTerm", Resources.ArgumentNull_String);

			return Provider.Search(blogId, searchTerm);
		}
	}
}