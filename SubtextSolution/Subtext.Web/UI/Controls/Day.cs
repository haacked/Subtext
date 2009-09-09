#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Displays all entries for a given day.
    /// </summary>
    public class Day : EntryList
    {
        private EntryDay bpd;
        protected Literal DateTitle;
        protected Repeater DayList;
        private Entry entry;
        protected HyperLink ImageLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="Day"/> class and sets 
        /// the DescriptionOnly property to false.
        /// </summary>
        public Day()
        {
            DescriptionOnly = false;
        }

        /// <summary>
        /// Sets the current day.
        /// </summary>
        /// <value>The current day.</value>
        public EntryDay CurrentDay
        {
            get { return bpd; }
            set { bpd = value; }
        }

        protected Entry Entry
        {
            get { return entry; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
                Visible = false;
            }
        }

        void DayList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            entry = e.Item.DataItem as Entry;
        }
    }
}