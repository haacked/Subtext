using System;
using Subtext.Common.Data;
using Subtext.Framework.Components;

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
	using System;

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

		protected Subtext.Web.UI.Controls.EntryList EntryStoryList;


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{

				//int catID = Globals.GetPostIDFromUrl(Request.Path);
				LinkCategory lc = Cacher.SingleCategory(CacheTime.Short,Context);
				
				int count = Request.QueryString["Show"] != null ? 0 :10;

				if(lc != null)
				{
					EntryCollection ec = Cacher.GetEntriesByCategory(count,CacheTime.Short,Context,lc.CategoryID);
					EntryStoryList.EntryListItems = ec;

					EntryStoryList.EntryListTitle = lc.Title;
					if(lc.HasDescription)
					{
						EntryStoryList.EntryListDescription = lc.Description;
					}
						
					if(count != 0 && ec != null && ec.Count == 10) //crappy. If the category has 10 entries, we will show the full archive link?
					{
						EntryStoryList.EntryListReadMoreText = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Full {0} Archive",lc.Title);
						EntryStoryList.EntryListReadMoreUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?Show=All",Request.Path);

					}

					Subtext.Web.UI.Globals.SetTitle(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1}",CurrentBlog.Title,lc.Title),Context);
				}

			}
		}
	}
}

