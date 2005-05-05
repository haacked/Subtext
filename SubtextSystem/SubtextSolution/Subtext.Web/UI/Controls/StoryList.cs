using System;
using Subtext.Common.Data;
using Subtext.Framework.Components;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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
						EntryStoryList.EntryListReadMoreText = string.Format("Full {0} Archive",lc.Title);
						EntryStoryList.EntryListReadMoreUrl = string.Format("{0}?Show=All",Request.Path);

					}

					Subtext.Web.UI.Globals.SetTitle(string.Format("{0} - {1}",CurrentBlog.Title,lc.Title),Context);
				}

			}
		}
	}
}

