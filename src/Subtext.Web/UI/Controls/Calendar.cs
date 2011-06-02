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
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Summary description for Calendar.
    /// </summary>
    public class Calendar : BaseControl
    {
        // The list of entries found for the month
        // Current index into _monthEntries.
        int _currentDateIndex;
        // Number of entries in _monthEntries
        int _dateCount;
        // True if the url is for a month and not a day (see ChooseSelectedDateFromUrl).
        protected bool IsUrlMonthMode;
        ICollection<Entry> _monthEntries;

        protected System.Web.UI.WebControls.Calendar entryCal;

        /// <summary>
        /// If the page is on a "day page" then have the calendar select the day the URL is on. 
        ///	Otherwise use the current day.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Context != null)
            {
                DateTime selectedDate = ChooseSelectedDateFromUrl();
                entryCal.ToolTip = selectedDate.ToShortDateString();

                // Setup current month			
                entryCal.SelectedDate = selectedDate;
                entryCal.VisibleDate = selectedDate;

                // setup prev/next months
                DateTime tempDate = selectedDate.AddMonths(-1);
                entryCal.PrevMonthText = string.Format(CultureInfo.InvariantCulture,
                                                       "<a href=\"{0}\" title=\"{1}\">&laquo;</a>",
                                                       Url.MonthUrl(tempDate), Resources.Calendar_PreviousMonth);
                tempDate = selectedDate.AddMonths(1);
                entryCal.NextMonthText = string.Format(CultureInfo.InvariantCulture,
                                                       "<a href=\"{0}\" title=\"{1}\">&raquo;</a>",
                                                       Url.MonthUrl(tempDate), Resources.Calendar_NextMonth);

                // fix the selected date if we're in "month mode"
                //		if (_isUrlMonthMode)
                //			entryCal.SelectedDate = NullValue.NullDateTime;

                LoadMonthData();
            }
        }

        /// <summary>
        /// Chooses the selected date from the url.
        /// </summary>
        /// <returns></returns>
        private DateTime ChooseSelectedDateFromUrl()
        {
            string dateStr;
            DateTime parsedDate;
            IsUrlMonthMode = false;

            // /YYYY/MM/DD.aspx ?
            var match = new Regex("(.*)(\\d{4})/(\\d{2})/(\\d{2}).aspx$");
            if (match.IsMatch(Request.RawUrl))
            {
                dateStr = match.Replace(Request.RawUrl, "$3-$4-$2");

                if (TryParseDateTime(dateStr, out parsedDate))
                {
                    return parsedDate;
                }
            }

            // /YYYY/MM.aspx ?
            match = new Regex("(.*)(\\d{4})/(\\d{2}).aspx$");
            if (match.IsMatch(Request.RawUrl))
            {
                dateStr = match.Replace(Request.RawUrl, "$3-01-$2");

                if (TryParseDateTime(dateStr, out parsedDate))
                {
                    IsUrlMonthMode = true;
                    return parsedDate;
                }
            }
            // If all else fails set the cal to today.
            return Blog.TimeZone.Now;
        }


        /// <summary>
        /// Attemps to parse the specified string as a DateTime.
        /// </summary>
        /// <param name="dateString">The string to parse.</param>
        /// <param name="parsedDate">The date if the string was parsed succesfully.</param>
        /// <returns>True if the string was parsed succesfully.</returns>
        private bool TryParseDateTime(string dateString, out DateTime parsedDate)
        {
            try
            {
                parsedDate = DateTime.ParseExact(dateString, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                return true;
            }
            catch (FormatException)
            {
                parsedDate = Blog.TimeZone.Now;
                return false;
            }
        }


        /// <summary>
        /// Load all entries for the selected month.
        /// </summary>
        private void LoadMonthData()
        {
            _monthEntries = Cacher.GetEntriesForMonth(entryCal.SelectedDate, SubtextContext);
            if (_monthEntries == null)
            {
                Trace.Warn("SubTextBlogCalendar Error: Cacher.GetMonth");
                _dateCount = 0;
            }
            else
            {
                _dateCount = _monthEntries.Count;
            }
            _currentDateIndex = _dateCount - 1;
        }


        /// <summary>
        /// As each day is rendered in the calendar, put a link if an entry exists for that day.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void entryCal_DayRender(object sender, DayRenderEventArgs e)
        {
            if (_currentDateIndex >= _dateCount || _currentDateIndex < 0)
            {
                return;
            }

            DateTime entryDate = _monthEntries.ElementAt(_currentDateIndex).DateCreatedUtc;
            DateTime calDate = e.Day.Date;

            if (IsSameDay(calDate, entryDate))
            {
                e.Cell.Text = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\">{1}</a>",
                                            Url.DayUrl(e.Day.Date), e.Day.Date.Day);

                // Go through the rest of the entries. (_monthEntries should always be sorted by DateCreated in descending order)
                do
                {
                    _currentDateIndex--;
                } while (_currentDateIndex > -1 &&
                        IsSameDay(e.Day.Date, _monthEntries.ElementAt(_currentDateIndex).DateCreatedUtc));
            }
        }

        /// <summary>
        /// Returns true if the two dates fall on the same day.
        /// </summary>
        static bool IsSameDay(DateTime date1, DateTime date2)
        {
            return (date1.Day == date2.Day) && (date1.Month == date2.Month) && (date1.Year == date2.Year);
        }


        /// <summary>
        /// Occurs when the user clicks on the next or previous month navigation controls on the title heading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void entryCal_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            //string url = CurrentBlog.UrlFormats.MonthUrl(e.NewDate);

            //Server.Transfer(url);
        }
    }
}