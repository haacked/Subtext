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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Summary description for PreviousNext.
    /// </summary>
    public class PreviousNext : BaseControl
    {
        protected Control LeftPipe;
        protected HyperLink MainLink;
        protected HyperLink NextLink;
        protected HyperLink PrevLink;
        protected Control RightPipe;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Get the entry
            Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);

            //if found
            if (entry != null)
            {
                //Sent entry properties
                MainLink.NavigateUrl = Url.BlogUrl();
                var entries = Cacher.GetPreviousNextEntry(entry.Id, PostType.BlogPost, SubtextContext);

                foreach (var navEntry in entries)
                {
                    navEntry.Blog = Blog;
                }

                //Remember, the NEXT entry is the MORE RECENT entry.
                switch (entries.Count)
                {
                    case 0:
                        {
                            //you have no entries. You should blog more
                            if (PrevLink != null)
                            {
                                PrevLink.Visible = false;
                            }
                            if (NextLink != null)
                            {
                                NextLink.Visible = false;
                            }
                            break;
                        }
                    case 1:
                        {
                            //since there is only one record, you are at an end
                            //Check EntryId to see if it is greater or less than
                            //the current ID
                            if (entries.First().DateSyndicated > entry.DateSyndicated)
                            {
                                //this is the oldest blog
                                if (PrevLink != null)
                                {
                                    PrevLink.Visible = false;
                                }
                                if (LeftPipe != null)
                                {
                                    LeftPipe.Visible = false;
                                }
                                SetNav(NextLink, entries.First());
                            }
                            else
                            {
                                //this is the latest blog
                                if (NextLink != null)
                                {
                                    NextLink.Visible = false;
                                }
                                if (RightPipe != null)
                                {
                                    RightPipe.Visible = false;
                                }
                                SetNav(PrevLink, entries.First());
                            }
                            break;
                        }
                    case 2:
                        {
                            //two records found. The first record will be NEXT
                            //the second record will be PREVIOUS
                            //This is because the query is sorted by EntryId
                            SetNav(NextLink, entries.First());
                            SetNav(PrevLink, entries.ElementAt(1));
                            break;
                        }
                }
            }
            else
            {
                //No post? Deleted? Help :)
                Controls.Clear();
                Controls.Add(
                    new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
            }
        }


        private void SetNav(HyperLink navLink, EntrySummary entry)
        {
            if (navLink == null)
            {
                return;
            }
            string format = navLink.Attributes["Format"];
            if (String.IsNullOrEmpty(format))
            {
                format = "{0}";
            }

            navLink.Attributes.Remove("Format");

            string entryTitle = HttpUtility.HtmlDecode(entry.Title);
            string sizeLimitText = navLink.Attributes["TextSizeLimit"];
            if (!String.IsNullOrEmpty(sizeLimitText))
            {
                int sizeLimit;
                if (int.TryParse(sizeLimitText, out sizeLimit))
                {
                    if (sizeLimit > 0 && sizeLimit < entryTitle.Length)
                    {
                        entryTitle = entryTitle.Substring(0, sizeLimit) + "...";
                    }
                }
            }
            navLink.Attributes.Remove("TextSizeLimit");

            navLink.Text = HttpUtility.HtmlEncode(string.Format(format, entryTitle));
            navLink.NavigateUrl = Url.EntryUrl(entry);
        }
    }
}