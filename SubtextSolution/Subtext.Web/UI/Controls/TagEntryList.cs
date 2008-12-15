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
using System.IO;
using System.Web;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public class TagEntryList : BaseControl
    {
        public bool DescriptionOnly
		{
			get { return EntryStoryList.DescriptionOnly; }
			set { EntryStoryList.DescriptionOnly = value; }
		}

		protected EntryList EntryStoryList;

        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                Uri url = HttpContext.Current.Request.Url;
                string tagName = HttpUtility.UrlDecode(url.Segments[url.Segments.Length - 2].Replace("/", ""));

                ICollection<Entry> et = Cacher.GetEntriesByTag(Count, CacheDuration.Short, tagName);
                EntryStoryList.EntryListItems = et;
                EntryStoryList.EntryListTitle = tagName;
                EntryStoryList.EntryListDescription = string.Format("There are {0} entries for the tag <em>{1}</em>", et.Count, tagName);

                Globals.SetTitle(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1}", CurrentBlog.Title, tagName), Context);
            }
        }
    }
}
