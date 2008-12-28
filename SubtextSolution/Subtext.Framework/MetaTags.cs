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
using Subtext.Framework.Providers;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to retrieve meta tags from the data store. MetaTags are simple enough 
	/// that generic collections are used instead of custom MetaTag objects.
	/// </summary>
	public static class MetaTags
	{
		public static int Create(MetaTag metaTag)
		{
            if (metaTag == null)
                throw new ArgumentNullException("metaTag", "The meta tag can not be NULL.");

            if (!metaTag.IsValid)
                throw new ArgumentException(metaTag.ValidationMessage);
			metaTag.Id = ObjectProvider.Instance().Create(metaTag);
		    
            return metaTag.Id;
		}

		public static bool Update(MetaTag metaTag)
		{
            if (metaTag == null)
                throw new ArgumentNullException("metaTag", "The meta tag can not be NULL.");

            if (!metaTag.IsValid)
                throw new ArgumentException(metaTag.ValidationMessage);
		    
            return ObjectProvider.Instance().Update(metaTag);
		}

		public static bool Delete(int metaTagId)
		{
			return ObjectProvider.Instance().DeleteMetaTag(metaTagId);
		}

		public static IPagedCollection<MetaTag> GetMetaTagsForBlog(Blog blog, int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetMetaTagsForBlog(blog, pageIndex, pageSize);
		}

        public static IPagedCollection<MetaTag> GetMetaTagsForEntry(Entry entry, int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetMetaTagsForEntry(entry, pageIndex, pageSize);
        }
	}
}