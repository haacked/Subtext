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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Plugins;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Framework.Security;
using Subtext.Framework.Text;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Control used to display a list of entries.
	/// </summary>
	public class EntryList : BaseControl
	{	
		const string linkToComments = "<a href=\"{0}#feedback\" title=\"View and Add Comments\" class=\"comments\">{1}{2}</a>";
		const string postdescWithComments = "posted @ <a href=\"{0}\" title = \"Permanent link to this post\">{1}</a> | <a href=\"{2}#feedback\" title = \"comments, pingbacks, trackbacks\" class=\"comments\">Feedback ({3})</a>";
		const string postdescWithNoComments = "posted @ <a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>";

        private string category;
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

		protected virtual void PostCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				if(entry != null)
				{
					//Raise event before any processing takes place
					SubtextEvents.OnEntryRendering(this, new EntryEventArgs(entry));

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

					SubtextEvents.OnEntryRendered(this, new EntryEventArgs(entry));
				}
			}
		}

		private static void BindAuthor(RepeaterItemEventArgs e, Entry entry)
		{
			Label author = e.Item.FindControl("author") as Label;
			if(author != null)
			{
				if(entry.Author != null)
				{
					author.Text = entry.Author.UserName;
				}
			}
		}

		private static void BindCommentCount(RepeaterItemEventArgs e, Entry entry)
		{
			Label commentCount = e.Item.FindControl("commentCount") as Label;
			if(commentCount != null)
			{
				if(Config.CurrentBlog.CommentsEnabled && entry.AllowComments)
				{
					if(entry.FeedBackCount == 0)
					{
						commentCount.Text = string.Format(linkToComments, entry.Url, "Add Comment", "");
					}
					else if(entry.FeedBackCount == 1)
					{
						commentCount.Text = string.Format(linkToComments, entry.Url, "One Comment", "");
					}
					else if(entry.FeedBackCount > 1)
					{
						commentCount.Text = string.Format(linkToComments, entry.Url, entry.FeedBackCount, " Comments");
					}
				}
			}
		}

		private static void BindPostDate(RepeaterItemEventArgs e, Entry entry)
		{
			Label postDate = e.Item.FindControl("postDate") as Label;
			if(postDate != null)
			{
				if(postDate.Attributes["Format"] != null)
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

		private static void BindPermalink(RepeaterItemEventArgs e, Entry entry)
		{
			Label permalink = e.Item.FindControl("permalink") as Label;
			if(permalink != null)
			{
				if(permalink.Attributes["Format"] != null)
				{
					permalink.Text = string.Format("<a href=\"{0}\" title=\"Permanent link to this post\">{1}</a>", entry.Url, entry.DateSyndicated.ToString(permalink.Attributes["Format"]));
					permalink.Attributes.Remove("Format");
				}
				else
				{
					permalink.Text = string.Format("<a href=\"{0}\" title=\"Permanent link to this post\">{1}</a>", entry.Url, entry.DateSyndicated.ToString("f"));
				}
			}
		}

		private static void BindPostDescription(RepeaterItemEventArgs e, Entry entry)
		{
			Literal PostDesc = (Literal)e.Item.FindControl("PostDesc");
			if(PostDesc != null)
			{
				if(entry.AllowComments)
				{
					PostDesc.Text = string.Format(postdescWithComments, entry.Url, entry.DateSyndicated.ToString("f"), entry.Url, entry.FeedBackCount);
				}
				else
				{
					PostDesc.Text = string.Format(postdescWithNoComments, entry.Url, entry.DateSyndicated.ToString("f"));
				}
			}
		}

		private static void BindPostCategories(RepeaterItemEventArgs e, Entry entry)
		{
			PostCategoryList postCategories = (PostCategoryList)e.Item.FindControl("Categories");
			if (postCategories != null)
			{
				postCategories.LinkCategories = Links.GetLinkCategoriesByPostId(entry.Id);
				postCategories.DataBind();
			}
		}

        public static string ShowTruncatedBody(Entry entry,int definedwordlimit)
        {
            StringBuilder returnstring = new StringBuilder("<p>");
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

                string[] words = entry.Body.Split(new Char[] { ' ' });
                if (words.GetUpperBound(0) <= 0) //Body has one or fewer words
                {
                    returnstring.Append(entry.Body);
                    // NO need for appended ... because
                    //the entire post length is only one word
                }
                else
                {
                    int wordlimit;
                    int actualnumberofwords = words.GetUpperBound(0) + 1;
                    //First 100 words or however many there actually are, whichever is less
                    if (actualnumberofwords < definedwordlimit)
                    {
                        wordlimit = actualnumberofwords;
                    }
                    else
                    {
                        wordlimit = definedwordlimit; //TODO: Make this configurable
                    }
                    for (int i = 0; i < wordlimit; i++)
                    {
                        returnstring.Append(words[i] + " ");
                    }
                    //truncate trailing space
                    returnstring.Remove(returnstring.Length -1, 1);
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
			Literal PostText = (Literal)e.Item.FindControl("PostText");
	
			if(DescriptionOnly) // like on the monthly archive page
			{
                if (entry.HasDescription)
                {
                    PostText.Text = string.Format(CultureInfo.InvariantCulture, "<p>{0}</p>", entry.Description);
                }
                //DF:  Description=Excerpt, if none, show first 100 words of post
                else
                {
                    PostText.Text = ShowTruncatedBody(entry,100);
                }
			}
			else
			{
				if(entry.HasDescription)
				{
					PostText.Text = entry.Description;
				}
				else
				{
					PostText.Text = entry.Body;
				}
			}
		}

		private static void BindTitle(RepeaterItemEventArgs e, Entry entry)
		{
			HyperLink title = e.Item.FindControl("TitleUrl") as HyperLink;
			if(title != null)
			{
                title.Text = HttpUtility.HtmlEncode(entry.Title);
				ControlHelper.SetTitleIfNone(title, "Click To View Entry.");
				title.NavigateUrl = entry.Url;
			}
		}

		// If the user is an admin AND the the skin 
		// contains an edit Hyperlink control, this 
		// will display the edit control.
		protected virtual void BindEditLink(Entry entry, RepeaterItemEventArgs e)
		{
			HyperLink editLink = e.Item.FindControl("editLink") as HyperLink;
			if(editLink != null)
			{
				if(SecurityHelper.IsAdmin)
				{
					editLink.Visible = true;
					if(editLink.Text.Length == 0 && editLink.ImageUrl.Length == 0)
					{
						//We'll slap on our little pencil icon.
						editLink.ImageUrl = BlogInfo.VirtualDirectoryRoot + "Images/edit.gif";
						ControlHelper.SetTitleIfNone(editLink, "Click to edit this entry.");
						editLink.NavigateUrl = UrlFormats.GetEditLink(entry);
					}
				}
				else
				{
					editLink.Visible = false;
				}
			}
		}

        private IList<Entry> entries;
        public IList<Entry> EntryListItems
		{
			get{return entries;}
			set{entries = value;}
		}

		private bool descriptionOnly = true;
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
		public bool DescriptionOnly
		{
			get{return descriptionOnly;}
			set{descriptionOnly = value;}
		}

		private string entryListTitle;
		public string EntryListTitle
		{
			get{return entryListTitle;}
			set{entryListTitle = value;}
		}

		private string _entryListDescription;
		public string EntryListDescription
		{
			get {return this._entryListDescription;}
			set {this._entryListDescription = value;}
		}

		private string _entryListReadMoreText;
		public string EntryListReadMoreText
		{
			get {return this._entryListReadMoreText;}
			set {this._entryListReadMoreText = value;}
		}

		private string _entryListReadMoreUrl;
		public string EntryListReadMoreUrl
		{
			get {return this._entryListReadMoreUrl;}
			set {this._entryListReadMoreUrl = value;}
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

            if (EntryListItems == null && !string.IsNullOrEmpty(Category))
            {
                // This EntryList is independent of an outside control and needs to
                //   populate its own EntryListItems.
                LinkCategory lc;
                if (StringHelper.IsNumeric(Category))
                {
                    int categoryID = Int32.Parse(Category);
                    lc = Cacher.SingleCategory(CacheDuration.Short, categoryID, false);
                }
                else
                {
                    lc = Cacher.SingleCategory(CacheDuration.Short, Category, false);
                }
                EntryListTitle = lc.Title;
                EntryListItems = Cacher.GetEntriesByCategory(0, CacheDuration.Short, lc.Id);
            }

			if(EntryListItems != null)
			{
                Literal entryCollectionTitle = this.FindControl("EntryCollectionTitle") as Literal;
                if(entryCollectionTitle != null)
                {
                    entryCollectionTitle.Text = EntryListTitle;
                }

                Literal entryCollectionDescription = this.FindControl("EntryCollectionDescription") as Literal;
                if(entryCollectionDescription != null)
                {
                    if(EntryListDescription != null)
                    {
                        entryCollectionDescription.Text = EntryListDescription;
                    }
                    else
                    {
                        entryCollectionDescription.Visible = false;
                    }
                }

                HyperLink entryListReadMoreUrl = this.FindControl("EntryCollectionReadMoreLink") as HyperLink;

                if(entryListReadMoreUrl != null)
                {
                    if(EntryListReadMoreText != null)
                    {
                        entryListReadMoreUrl.Text = EntryListReadMoreText;
                        entryListReadMoreUrl.NavigateUrl = EntryListReadMoreUrl;
                    }
                    else
                    {
                        entryListReadMoreUrl.Visible = false;
                    }
                }

                Repeater entryRepeater = this.FindControl("Entries") as Repeater;
                if(entryRepeater != null)
                {
                    entryRepeater.DataSource = EntryListItems;
                    entryRepeater.DataBind();
                }
			}
		}
	}
}


