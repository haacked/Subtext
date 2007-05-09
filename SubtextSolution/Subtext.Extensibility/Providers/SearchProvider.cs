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
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace Subtext.Extensibility.Providers
{
	public abstract class SearchProvider : ProviderBase
	{
		private static SearchProvider provider;
		private static GenericProviderCollection<SearchProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<SearchProvider>("Search", out provider);

		/// <summary>
		/// Initializes this provider, setting the connection string.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="config">Config value.</param>
		public override void Initialize(string name, NameValueCollection config)
		{
			this.connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", config);
			base.Initialize(name, config);
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value></value>
		protected string ConnectionString
		{
			get { return this.connectionString; }
			set { this.connectionString = value; }
		}
		
		private string connectionString;
		
		/// <summary>
		/// Searches the specified blog for items that match the search term.
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="searchTerm"></param>
		/// <returns></returns>
		public abstract IList<SearchResult> Search(int blogId, string searchTerm);
	}
	
	public struct SearchResult
	{
        string title;
        Uri url;
        
        public SearchResult(string title, Uri url)
		{
			this.title = title;
			this.url = url;
		}
		
		public string Title
		{
			get { return this.title; }
		}

		public Uri Url
		{
			get { return this.url; }
		}

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(SearchResult))
                return false;

            SearchResult s = (SearchResult)obj;
            return ((this.title == s.title) && (this.url == s.url));
        }

        public override int GetHashCode()
        {
            return (this.title.GetHashCode() ^ this.url.GetHashCode());
        }

        public static bool operator ==(SearchResult s1, SearchResult s2)
        {
            return ((s1.title == s2.title) && (s1.url == s2.url));
        }

        public static bool operator !=(SearchResult s1, SearchResult s2)
        {
            return !(s1 == s2);
        }
	}
}
