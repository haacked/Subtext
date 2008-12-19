using System.Collections.Generic;
using System.Data;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace Subtext.Framework.Providers
{
	public class EntrySearchProvider : SearchProvider
	{
        StoredProcedures _procedures = null;
        
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
        {
            base.Initialize(name, configValue);
            
        }

        protected override string ConnectionString
        {
            get
            {
                return base.ConnectionString;
            }
            set
            {
                if (value != null) {
                    _procedures = new StoredProcedures(value);
                }
                base.ConnectionString = value;
            }
        }

		/// <summary>
		/// Searches the specified blog for items that match the search term.
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="searchTerm"></param>
		/// <returns></returns>
		public override ICollection<SearchResult> Search(int blogId, string searchTerm)
		{
            ICollection<SearchResult> results = new List<SearchResult>();

            using (IDataReader reader = _procedures.SearchEntries(blogId, searchTerm, Config.CurrentBlog.TimeZone.Now)) {
                while (reader.Read())
                {
                    Entry foundEntry = DataHelper.LoadEntry(reader, true);
                    results.Add(new SearchResult(foundEntry.Title, foundEntry.FullyQualifiedUrl));
                }
            }
            return results;
		}
	}
}
