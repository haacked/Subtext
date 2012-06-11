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
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    public class TagEntryList : BaseControl
    {
        protected EntryList EntryStoryList;

        public bool DescriptionOnly
        {
            get { return EntryStoryList.DescriptionOnly; }
            set { EntryStoryList.DescriptionOnly = value; }
        }

        public int Count { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                var tagName = RouteValues["tag"] as string;

                ICollection<Entry> et = Cacher.GetEntriesByTag(Count, tagName, SubtextContext);
                EntryStoryList.EntryListItems = et;
                EntryStoryList.EntryListTitle = tagName;
                EntryStoryList.EntryListDescription = string.Format(CultureInfo.InvariantCulture,
                                                                    Resources.TagEntryList_NoEntriesForTag, et.Count,
                                                                    tagName);

                Globals.SetTitle(string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Blog.Title, tagName), Context);
            }
        }
    }
}