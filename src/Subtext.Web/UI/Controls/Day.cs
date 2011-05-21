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
        protected Literal DateTitle;
        protected Repeater DayList;
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
        public EntryDay CurrentDay { get; set; }

        protected Entry Entry { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (CurrentDay != null)
            {
                DayList.ItemDataBound += DayList_ItemDataBound;

                if (ImageLink != null)
                {
                    ImageLink.NavigateUrl = Url.DayUrl(CurrentDay.BlogDay);

                    if (ImageLink.ImageUrl.StartsWith("~/"))
                    {
                        ImageLink.ImageUrl = Url.ResolveUrl(ImageLink.ImageUrl);
                    }

                    ControlHelper.SetTitleIfNone(ImageLink, "Click to see entries for this day.");
                }

                if (DateTitle != null)
                    DateTitle.Text = CurrentDay.BlogDay.ToLongDateString();
                DayList.DataSource = CurrentDay;
                DayList.DataBind();
            }
            else
            {
                Visible = false;
            }
        }

        void DayList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Entry = e.Item.DataItem as Entry;
        }
    }
}