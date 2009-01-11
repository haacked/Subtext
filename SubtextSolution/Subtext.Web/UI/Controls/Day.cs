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
using Subtext.Framework.Components;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Displays all entries for a given day.
	/// </summary>
	public class Day : EntryList
	{
		protected System.Web.UI.WebControls.Repeater DayList;
		protected System.Web.UI.WebControls.HyperLink ImageLink;
		protected System.Web.UI.WebControls.Literal  DateTitle;

		private EntryDay bpd;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Day"/> class and sets 
		/// the DescriptionOnly property to false.
		/// </summary>
		public Day() : base()
		{
			this.DescriptionOnly = false;	
		}

		/// <summary>
		/// Sets the current day.
		/// </summary>
		/// <value>The current day.</value>
		public EntryDay CurrentDay
		{
			get{return bpd;}
			set{bpd = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(bpd != null)
			{
				DayList.ItemDataBound += DayList_ItemDataBound;

				ImageLink.NavigateUrl = Url.DayUrl(bpd.BlogDay);
				ControlHelper.SetTitleIfNone(ImageLink, "Click to see entries for this day.");
				DateTitle.Text = bpd.BlogDay.ToLongDateString();
				DayList.DataSource = bpd;
				DayList.DataBind();
			}
			else
			{
				this.Visible = false;
			}
		}

		private Entry entry;

		protected Entry Entry
		{
			get { return this.entry; }
		}

		void DayList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			entry = e.Item.DataItem as Entry;
		}

	}
}

