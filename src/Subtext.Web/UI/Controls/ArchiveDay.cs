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
using System.Globalization;
using Subtext.Framework.Data;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for ArchiveDay.
    /// </summary>
    public class ArchiveDay : BaseControl
    {
        protected Day SingleDay;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                try
                {
                    DateTime dayDate = SubtextContext.RequestContext.GetDateFromRequest();
                    SingleDay.CurrentDay = Cacher.GetEntriesForDay(dayDate, SubtextContext);
                    Globals.SetTitle(
                        string.Format(CultureInfo.InvariantCulture, "{0} - {1} Entries", Blog.Title,
                                      dayDate.ToString("D", CultureInfo.CurrentCulture)), Context);
                }
                catch (FormatException)
                {
                    //Somebody probably is messing with the url.
                    //404 is set in filenotfound - DF
                    Response.Redirect("~/SystemMessages/FileNotFound.aspx");
                }
            }
        }
    }
}