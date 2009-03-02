using System;
using System.Collections.Generic;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Web;

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

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for ArchiveDay.
	/// </summary>
	public  class CategoryEntryList : BaseControl
	{
		public bool DescriptionOnly
		{
			get { return EntryStoryList.DescriptionOnly; }
			set { EntryStoryList.DescriptionOnly = value; }
		}

		protected EntryList EntryStoryList;


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{
				Blog info = Blog;

				LinkCategory lc = Cacher.SingleCategory(CacheDuration.Short, Blog);
				
				int count = Request.QueryString["Show"] != null ? 0 : info.CategoryListPostCount;//as of 3sep2006, this is a configurable option. 
				//However, we retain the ability to overide the CategoryListPostCount setting via the query string, as usual.

				if(lc == null)
				{
					HttpHelper.SetFileNotFoundResponse();
					return;
				}
				
				ICollection<Entry> ec = Cacher.GetEntriesByCategory(count, CacheDuration.Short, lc.Id, info);
				EntryStoryList.EntryListItems = ec;

				EntryStoryList.EntryListTitle = lc.Title;
				if(lc.HasDescription)
				{
					EntryStoryList.EntryListDescription = lc.Description;
				}
					
				if(count != 0 && ec != null && ec.Count == info.CategoryListPostCount) //crappy. If the only category has #CategoryListPostCount entries, we will show the full archive link?
				{
					EntryStoryList.EntryListReadMoreText = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Full {0} Archive",lc.Title);
					EntryStoryList.EntryListReadMoreUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?Show=All",Request.Path);

				}

				Globals.SetTitle(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1}", Blog.Title, lc.Title), Context);
			}
		}
	}
}

