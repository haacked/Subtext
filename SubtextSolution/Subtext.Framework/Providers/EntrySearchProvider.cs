using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace Subtext.Framework.Providers
{
	public class EntrySearchProvider : SearchProvider
	{
		/// <summary>
		/// Searches the specified blog for items that match the search term.
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="searchTerm"></param>
		/// <returns></returns>
		public override IList<SearchResult> Search(int blogId, string searchTerm)
		{
			string storedProc = "subtext_SearchEntries";

			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, blogId),
                DataHelper.MakeInParam("@CurrentDateTime", SqlDbType.DateTime, 4, Config.CurrentBlog.TimeZone.Now),
				DataHelper.MakeInParam("@SearchStr", searchTerm)
			};

			IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, storedProc, p);

			IList<SearchResult> results = new List<SearchResult>();
			while(reader.Read())
			{
				Entry foundEntry = DataHelper.LoadEntry(reader, true);
				results.Add(new SearchResult(foundEntry.Title, foundEntry.FullyQualifiedUrl));
			}

			return results;
		}
	}
}
