using System;
using System.Collections.Generic;
using Subtext.Extensibility.Providers;

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
			return Provider.Search(blogId, searchTerm);
		}
	}
}