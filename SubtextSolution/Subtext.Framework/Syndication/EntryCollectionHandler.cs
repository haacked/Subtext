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
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// </summary>
	public abstract class EntryCollectionHandler<T> : BaseSyndicationHandler<T>
	{
		protected abstract IList<T> GetFeedEntries();

		protected override bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
		
			if(dt != null)
			{
                IList<T> ec = GetFeedEntries();

				if(ec != null && ec.Count > 0)
				{
				    //Get the first entry.
				    T entry = default(T);
				    //TODO: Probably change GetFeedEntries to return IList<Entry>
				    foreach(T en in ec)
				    {
				        entry = en;
				        break;
				    }
					return DateTime.Compare(DateTime.Parse(dt), ConvertLastUpdatedDate(GetItemCreatedDate(entry))) == 0;
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

		/// <summary>
		/// Gets the item created date.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract DateTime GetItemCreatedDate(T item);
	}
}

