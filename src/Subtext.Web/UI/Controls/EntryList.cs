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
using System.Text;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Security;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Control used to display a list of entries.
    /// </summary>
    public class EntryList : BaseControl
    {
        const string LinkToEnclosure = "<a href=\"{0}\" title = \"{1}\" class=\"enclosure-link\">{2}</a>{3}";

        static readonly string LinkToComments = "<a href=\"{0}#feedback\" title=\"" +
                                                Resources.EntryList_ViewAndAddComments +
                                                "\" class=\"comments\">{1}{2}</a>";

        static readonly string PostdescWithComments = "posted @ <a href=\"{0}\" title = \"" +
                                                      Resources.EntryList_PermanentLink +
                                                      "\">{1}</a> | <a href=\"{2}#feedback\" title = \"comments, pingbacks, trackbacks\" class=\"comments\">Feedback ({3})</a>";

        static readonly string PostdescWithNoComments = "posted @ <a href=\"{0}\" title = \"" +
                                                        Resources.EntryList_PermanentLink + "\">{1}</a>";

        public EntryList()
        {
            DescriptionOnly = true;
        }

        public string Category { get; set; }
        public ICollection<Entry> EntryListItems { get; set; }

        /// <summary>
        /// <para>
        /// If true, then the EntryList will only show the description 
        /// for an entry, if a description exists.
        /// If a description does NOT exist, then show the first 100 words of the post 
        /// followed by ...  TODO: make the number of words configurable.
        /// </para>
        /// <para>
        /// If false, then the description is show. But if the description 
        /// does not exist, the full text will be shown.
        /// </para>
        /// </summary>
        public bool DescriptionOnly { get; set; }

        public string EntryListTitle { get; set; }

        public string EntryListDescription { get; set; }

        public string EntryListReadMoreText { get; set; }

        public string EntryListReadMoreUrl { get; set; }

        protected virtual void PostCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var entry = (Entry)e.Item.DataItem;
                if (entry != null)
                {
                    // Each of these methods make sure to check that the 
                    // control to bind actually exists on the skin.
                    BindTitle(e, entry);
                    BindEditLink(entry, e);
                    BindPostText(e, entry);
                    BindPostDescription(e, entry);
                    BindPostCategories(e, entry);
                    BindPermalink(e, entry);
                    BindPostDate(e, entry);
                    BindCommentCount(e, entry);
                    BindAuthor(e, entry);
                    BindCurrentEntryControls(entry, e.Item);
                    BindEnclosure(e, entry);
                }
            }
        }

        private static void BindEnclosure(RepeaterItemEventArgs e, Entry entry)
        {
            var enclosure = (Label)e.Item.FindControl("Enclosure");
            if (enclosure != null)
            {
                if (entry.Enclosure != null && entry.Enclosure.ShowWithPost)
                {
                    bool displaySize;
                    Boolean.TryParse(enclosure.Attributes["DisplaySize"], out displaySize);

                    string sizeStr = null;
                    if (displaySize)
                    {
                        sizeStr = string.Format(" ({0})", entry.Enclosure.FormattedSize);
                    }
                    enclosure.Text = string.Format(LinkToEnclosure, entry.Enclosure.Url, entry.Enclosure.Title,
                                                   entry.Enclosure.Title, sizeStr);
                }
            }
        }


        private static void BindAuthor(RepeaterItemEventArgs e, Entry entry)
        {
            var author = e.Item.FindControl("author") as Label;
            if (author != null)
            {
                if (!String.IsNullOrEmpty(entry.Author))
                {
                    author.Text = entry.Author;
                }
            }
        }

        private void BindCommentCount(RepeaterItemEventArgs e, Entry entry)
        {
            var commentCount = e.Item.FindControl("commentCount") as Label;
            if (commentCount != null)
            {
                if (Blog.CommentsEnabled && entry.AllowComments)
                {
                    //TODO: Consider a specific url helper method for link to feedback section.
                    string entryUrl = Url.EntryUrl(entry);
                    if (entry.FeedBackCount == 0)
                    {
                        commentCount.Text = string.Format(CultureInfo.InvariantCulture, LinkToComments, entryUrl,
                                                          Resources.EntryList_AddComment, string.Empty);
                    }
                    else if (entry.FeedBackCount == 1)
                    {
                        commentCount.Text = string.Format(CultureInfo.InvariantCulture, LinkToComments, entryUrl,
                                                          Resources.EntryList_OneComment, string.Empty);
                    }
                    else if (entry.FeedBackCount > 1)
                    {
                        commentCount.Text = string.Format(LinkToComments, entryUrl, entry.FeedBackCount,
                                                          Resources.EntryList_CommentsPlural);
                    }
                }
            }
        }

        private static void BindPostDate(RepeaterItemEventArgs e, Entry entry)
        {
            var postDate = e.Item.FindControl("postDate") as Label;
            if (postDate != null)
            {
                if (postDate.Attributes["Format"] != null)
                {
                    postDate.Text = entry.DateSyndicated.ToString(postDate.Attributes["Format"]);
                    postDate.Attributes.Remove("Format");
                }
                else
                {
                    postDate.Text = entry.DateSyndicated.ToString("f");
                }
            }
        }

        private void BindPermalink(RepeaterItemEventArgs e, Entry entry)
        {
            var permalink = e.Item.FindControl("permalink") as Label;
            if (permalink != null)
            {
                string entryUrl = Url.EntryUrl(entry);
                if (permalink.Attributes["Format"] != null)
                {
                    permalink.Text = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" title=\"{2}\">{1}</a>",
                                                   entryUrl,
                                                   entry.DateSyndicated.ToString(permalink.Attributes["Format"]),
                                                   Resources.EntryList_PermanentLink);
                    permalink.Attributes.Remove("Format");
                }
                else
                {
                    permalink.Text = string.Format(CultureInfo.InvariantCulture, "<a href=\"{0}\" title=\"{2}\">{1}</a>",
                                                   entryUrl, entry.DateSyndicated.ToString("f"),
                                                   Resources.EntryList_PermanentLink);
                }
            }
        }

        private void BindPostDescription(RepeaterItemEventArgs e, Entry entry)
        {
            var postDesc = (Literal)e.Item.FindControl("PostDesc");
            if (postDesc != null)
            {
                string entryUrl = Url.EntryUrl(entry);
                if (entry.AllowComments)
                {
                    postDesc.Text = string.Format(PostdescWithComments, entryUrl, entry.DateSyndicated.ToString("f"),
                                                  entryUrl, entry.FeedBackCount);
                }
                else
                {
                    postDesc.Text = string.Format(PostdescWithNoComments, entryUrl, entry.DateSyndicated.ToString("f"));
                }
            }
        }

        private void BindPostCategories(RepeaterItemEventArgs e, IIdentifiable entry)
        {
            var postCategories = (PostCategoryList)e.Item.FindControl("Categories");
            if (postCategories != null)
            {
                postCategories.LinkCategories = Repository.GetLinkCategoriesByPostId(entry.Id);
                postCategories.DataBind();
            }
        }

        public static string ShowTruncatedBody(Entry entry, int definedwordlimit)
        {
            var returnstring = new StringBuilder("<p>");
            if (entry.Body == null)
            {
                returnstring.Append("");
            }
            else if (entry.Body.Length == 0)
            {
                returnstring.Append(entry.Body);
            }
            else
            {
                //We're counting words so HTML will get in the way
                //unless somebody has a better idea....
                entry.Body = HtmlHelper.RemoveHtml(entry.Body);

                string[] words = entry.Body.Split(new[] { ' ' });
                if (words.GetUpperBound(0) <= 0) //Body has one or fewer words
                {
                    returnstring.Append(entry.Body);
                    // NO need for appended ... because
                    //the entire post length is only one word
                }
                else
                {
                    int actualnumberofwords = words.GetUpperBound(0) + 1;
                    //First 100 words or however many there actually are, whichever is less
                    int wordlimit = actualnumberofwords < definedwordlimit ? actualnumberofwords : definedwordlimit;
                    for (int i = 0; i < wordlimit; i++)
                    {
                        returnstring.Append(words[i] + " ");
                    }
                    //truncate trailing space
                    returnstring.Remove(returnstring.Length - 1, 1);
                    if (actualnumberofwords > definedwordlimit) // add ... if there is more to the body
                    {
                        returnstring.Append("...");
                    }
                }
            }
            returnstring.Append("</p>");
            return string.Format(CultureInfo.InvariantCulture, "{0}", returnstring);
        }

        private void BindPostText(RepeaterItemEventArgs e, Entry entry)
        {
            var postText = (Literal)e.Item.FindControl("PostText");

            if (DescriptionOnly) // like on the monthly archive page
            {
                postText.Text = entry.HasDescription ? string.Format(CultureInfo.InvariantCulture, "<p>{0}</p>", entry.Description) : ShowTruncatedBody(entry, 100);
            }
            else
            {
                postText.Text = entry.HasDescription ? entry.Description : entry.Body;
            }
        }

        private void BindTitle(RepeaterItemEventArgs e, Entry entry)
        {
            var title = e.Item.FindControl("TitleUrl") as HyperLink;
            if (title != null)
            {
                title.Text = entry.Title;
                ControlHelper.SetTitleIfNone(title, Resources.EntryList_ClickToView);
                title.NavigateUrl = Url.EntryUrl(entry);
            }
        }

        // If the user is an admin AND the the skin 
        // contains an edit Hyperlink control, this 
        // will display the edit control.
        protected virtual void BindEditLink(Entry entry, RepeaterItemEventArgs e)
        {
            var editLink = e.Item.FindControl("editLink") as HyperLink;
            var editInWlwLink = e.Item.FindControl("editInWlwLink") as HyperLink;

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


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (EntryListItems == null && !string.IsNullOrEmpty(Category))
            {
                // This EntryList is independent of an outside control and needs to
                //   populate its own EntryListItems.
                LinkCategory lc;
                if (Category.IsNumeric())
                {
                    int categoryId = Int32.Parse(Category);
                    lc = Cacher.SingleCategory(categoryId, false, SubtextContext);
                }
                else
                {
                    lc = Cacher.SingleCategory(Category, false, SubtextContext);
                }
                EntryListTitle = lc.Title;
                EntryListItems = Cacher.GetEntriesByCategory(0, lc.Id, SubtextContext);
            }

            if (EntryListItems != null)
            {
                var entryCollectionTitle = FindControl("EntryCollectionTitle") as Literal;
                if (entryCollectionTitle != null)
                {
                    entryCollectionTitle.Text = EntryListTitle;
                }

                var entryCollectionDescription = FindControl("EntryCollectionDescription") as Literal;
                if (entryCollectionDescription != null)
                {
                    if (EntryListDescription != null)
                    {
                        entryCollectionDescription.Text = EntryListDescription;
                    }
                    else
                    {
                        entryCollectionDescription.Visible = false;
                    }
                }

                var entryListReadMoreUrl = FindControl("EntryCollectionReadMoreLink") as HyperLink;

                if (entryListReadMoreUrl != null)
                {
                    if (EntryListReadMoreText != null)
                    {
                        entryListReadMoreUrl.Text = EntryListReadMoreText;
                        entryListReadMoreUrl.NavigateUrl = EntryListReadMoreUrl;
                    }
                    else
                    {
                        entryListReadMoreUrl.Visible = false;
                    }
                }

                var entryRepeater = FindControl("Entries") as Repeater;
                if (entryRepeater != null)
                {
                    entryRepeater.DataSource = EntryListItems;
                    entryRepeater.DataBind();
                }
            }
        }
    }
}