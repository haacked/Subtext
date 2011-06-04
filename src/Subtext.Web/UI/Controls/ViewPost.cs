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
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Tracking;
using Subtext.Web.Controls;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Control used to view a single blog post.
    /// </summary>
    public partial class ViewPost : BaseControl
    {
        const string LinkToComments = "<a href=\"{0}#feedback\" title=\"View and Add Comments\">{1}{2}</a>";
        const string LinkToEnclosure = "<a href=\"{0}\" title = \"{1}\">{2}</a>{3}";

        /// <summary>
        /// Loads the entry specified by the URL.  If the user is an 
        /// admin and the skin supports it, will also display an edit 
        /// link that navigates to the admin section and allows the 
        /// admin to edit the post.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Get the entry
            Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);

            //if found
            if (entry != null)
            {
                BindCurrentEntryControls(entry, this);

                DisplayEditLink(entry);

                var statistics = DependencyResolver.Current.GetService<IStatisticsService>();
                statistics.RecordWebView(new EntryView { EntryId = entry.Id, BlogId = Blog.Id });

                //Set the page title
                Globals.SetTitle(entry.Title, Context);

                //Sent entry properties
                TitleUrl.Text = entry.Title;
                ControlHelper.SetTitleIfNone(TitleUrl, "Title of this entry.");
                TitleUrl.NavigateUrl = Url.EntryUrl(entry);
                Body.Text = entry.Body;
                if (PostDescription != null)
                {
                    PostDescription.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                                                         entry.DateSyndicated.ToLongDateString(),
                                                         entry.DateSyndicated.ToShortTimeString());
                }
                Trace.Write("loading categories");
                if (Categories != null)
                {
                    Categories.LinkCategories = Repository.GetLinkCategoriesByPostId(entry.Id);
                    Categories.DataBind();
                }

                if (date != null)
                {
                    string entryUrl = Url.EntryUrl(entry);
                    if (date.Attributes["Format"] != null)
                    {
                        date.Text = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" title=\"{2}\">{1}</a>",
                                                  entryUrl, entry.DateSyndicated.ToString(date.Attributes["Format"]),
                                                  Resources.EntryList_PermanentLink);
                        date.Attributes.Remove("Format");
                    }
                    else
                    {
                        date.Text = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" title=\"{2}\">{1}</a>",
                                                  entryUrl, entry.DateSyndicated.ToString("f"),
                                                  Resources.EntryList_PermanentLink);
                    }
                }

                if (commentCount != null)
                {
                    if (Blog.CommentsEnabled && entry.AllowComments)
                    {
                        string entryUrl = Url.EntryUrl(entry);
                        if (entry.FeedBackCount == 0)
                        {
                            commentCount.Text = string.Format(LinkToComments, entryUrl, Resources.EntryList_AddComment,
                                                              string.Empty);
                        }
                        else if (entry.FeedBackCount == 1)
                        {
                            commentCount.Text = string.Format(LinkToComments, entryUrl, Resources.EntryList_OneComment,
                                                              string.Empty);
                        }
                        else if (entry.FeedBackCount > 1)
                        {
                            commentCount.Text = string.Format(LinkToComments, entryUrl, entry.FeedBackCount,
                                                              Resources.EntryList_CommentsPlural);
                        }
                    }
                }

                BindEnclosure(entry);

                //Set Pingback/Trackback 
                if (PingBack == null)
                {
                    PingBack = Page.FindControl("pinbackLinkTag") as Literal;
                }

                if (PingBack != null)
                {
                    PingBack.Text = TrackHelpers.GetPingPackTag(Url);
                }

                if (TrackBack != null)
                {
                    TrackBack.Text = TrackHelpers.TrackBackTag(entry, Blog, Url);
                }
                DataBind();
            }
            else
            {
                //No post? Deleted? Help :)
                Controls.Clear();
                Controls.Add(new LiteralControl(Resources.ViewPost_EntryNotFound));
            }
        }

        private void BindEnclosure(Entry entry)
        {
            if (Enclosure != null)
            {
                if (entry.Enclosure != null && entry.Enclosure.ShowWithPost)
                {
                    bool displaySize;
                    Boolean.TryParse(Enclosure.Attributes["DisplaySize"], out displaySize);

                    string sizeStr = "";
                    if (displaySize)
                    {
                        sizeStr = string.Format(" ({0})", entry.Enclosure.FormattedSize);
                    }
                    Enclosure.Text = string.Format(LinkToEnclosure, entry.Enclosure.Url, entry.Enclosure.Title,
                                                   entry.Enclosure.Title, sizeStr);
                }
            }
        }

        // If the user is an admin AND the the skin 
        // contains an edit Hyperlink control, this 
        // will display the edit control.
        private void DisplayEditLink(IIdentifiable entry)
        {
            ExecDisplayEditLink(editLink, AdminUrl.PostsEdit(entry.Id).ToString(), Url.EditIconUrl().ToString());
            ExecDisplayEditLink(editInWlwLink, Url.WindowsLiveWriterEditUrl(entry.Id, this.Blog), Url.EditInWlwIconUrl().ToString());
        }

        private void ExecDisplayEditLink(HyperLink link, string editUrl, string editImageUrl)
        {
            if (link != null)
            {
                if (User.IsAdministrator())
                {
                    link.Visible = true;
                    link.NavigateUrl = editUrl;
                    ControlHelper.SetTitleIfNone(link, Resources.ViewPost_EditThisEntry);

                    if (String.IsNullOrEmpty(link.Text) && String.IsNullOrEmpty(link.ImageUrl))
                    {
                        //We'll slap on our little pencil icon.
                        link.ImageUrl = editImageUrl;
                    }
                }
                else
                {
                    link.Visible = false;
                }
            }
        }
    }
}