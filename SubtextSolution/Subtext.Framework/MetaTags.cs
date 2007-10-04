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
				throw new ArgumentNullException("metaTag", "MetaTag cannot be null.");
			
			if (metaTag.Content == null)
				throw new ArgumentNullException("metaTag", "Content cannot be null.");
			return ObjectProvider.Instance().Create(metaTag);
		}

		public static bool Update(MetaTag metaTag)
		{
			if (metaTag == null)
				throw new ArgumentNullException("metaTag", "Can't update a null meta tag.");
			return ObjectProvider.Instance().Update(metaTag);
		}

		public static bool Delete(int metaTagId)
		{
			ObjectProvider.Instance().Delete(metaTagId);
			return true;
		}

		public static IList<MetaTag> GetMetaTagsForBlog(BlogInfo blog)
		{
			return ObjectProvider.Instance().GetMetaTagsForBlog(blog);
		}

        public static IList<MetaTag> GetMetaTagsForEntry(Entry entry)
        {
            return ObjectProvider.Instance().GetMetaTagsForEntry(entry);
        }
	}
}