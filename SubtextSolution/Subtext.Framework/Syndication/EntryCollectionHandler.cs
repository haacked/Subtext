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
using Subtext.Framework.Components;
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// </summary>
	public abstract class EntryCollectionHandler : Subtext.Framework.Syndication.BaseSyndicationHandler
	{
		/// <summary>
		/// Creates a new <see cref="EntryCollectionHandler"/> instance.
		/// </summary>
	    protected EntryCollectionHandler()
		{
		}

        protected abstract IList<Entry> GetFeedEntries();

		protected override bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
		
			if(dt != null)
			{
                IList<Entry> ec = GetFeedEntries();

				if(ec != null && ec.Count > 0)
				{
				    //Get the first entry.
				    Entry entry = null;
				    //TODO: Probably change GetFeedEntries to return IList<Entry>
				    foreach(Entry en in ec)
				    {
				        entry = en;
				        break;
				    }
					return DateTime.Compare(DateTime.Parse(dt),this.ConvertLastUpdatedDate(entry.DateCreated)) == 0;
				}
			}
			return false;			
		}

		/// <summary>
		/// Returns the key used to cache this feed.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Date last viewed feed item published.</param>
		/// <returns></returns>
		protected override string CacheKey(DateTime dateLastViewedFeedItemPublished)
		{
			return null;
		}


		protected override void Cache(CachedFeed feed)
		{

		}


		//By default, we will assume the cached data objects will be used else where
		protected override bool IsHttpCacheOK()
		{
			return false;
		}

	



	}
}

