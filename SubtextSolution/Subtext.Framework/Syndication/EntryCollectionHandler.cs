#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// </summary>
    public abstract class EntryCollectionHandler<T> : BaseSyndicationHandler
    {
        protected EntryCollectionHandler(ISubtextContext subtextContext) : base(subtextContext)
        {
        }

        protected abstract ICollection<T> GetFeedEntries();

        protected override bool IsLocalCacheOk()
        {
            string dt = LastModifiedHeader;

            if(dt != null)
            {
                ICollection<T> ec = GetFeedEntries();

                if(ec != null && ec.Count > 0)
                {
                    //Get the first entry.
                    T entry = default(T);
                    //TODO: Probably change GetFeedEntries to return ICollection<Entry>
                    foreach(T en in ec)
                    {
                        entry = en;
                        break;
                    }
                    return
                        DateTime.Compare(DateTime.Parse(dt, CultureInfo.InvariantCulture),
                                         ConvertLastUpdatedDate(GetItemCreatedDate(entry))) == 0;
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
        protected override bool IsHttpCacheOk()
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