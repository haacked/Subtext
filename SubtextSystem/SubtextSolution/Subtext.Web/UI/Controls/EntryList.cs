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

using System;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Format;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Control used to display a list of entries.
	/// </summary>
	public class EntryList : BaseControl
	{
		protected System.Web.UI.WebControls.Repeater Entries;
		protected System.Web.UI.WebControls.Literal EntryCollectionTitle;
		protected System.Web.UI.WebControls.Literal EntryCollectionDescription;
		protected System.Web.UI.WebControls.HyperLink EntryCollectionReadMoreLink;

		const string linkToComments = "<a href=\"{0}#FeedBack\" title=\"View and Add Comments\">{1}{2}</a>";
		const string postdescWithComments = "posted @ <a href=\"{0}\" title = \"Permanent link to this post\">{1}</a> | <a href=\"{2}#FeedBack\" title = \"comments, pingbacks, trackbacks\">Feedback ({3})</a>";
		const string postdescWithNoComments = "posted @ <a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>";
		
		protected virtual void PostCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				if(entry != null)
				{
					HyperLink title = e.Item.FindControl("TitleUrl") as HyperLink;
					if(title != null)
					{
						title.Text = entry.Title;
						title.NavigateUrl = entry.Link;
					}

					DisplayEditLink(entry, e);

					Literal PostText = (Literal)e.Item.FindControl("PostText");

					if(DescriptionOnly)
					{
						if(entry.HasDescription)
						{
						
							PostText.Text = string.Format(System.Globalization.CultureInfo.InvariantCulture, "<p>{0}</p>",entry.Description);
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
					
					Literal PostDesc = (Literal)e.Item.FindControl("PostDesc");
					if(PostDesc != null)
					{
						
						if(entry.AllowComments)
						{
							PostDesc.Text = string.Format(postdescWithComments,entry.Link,entry.DateCreated.ToString("f"),entry.Link,entry.FeedBackCount);
						}
						else
						{
							PostDesc.Text = string.Format(postdescWithNoComments, entry.Link, entry.DateCreated.ToString("f"));
						}
					}

					Label permalink = e.Item.FindControl("permalink") as Label;
					if(permalink != null)
					{
						if(permalink.Attributes["Format"] != null)
						{
							permalink.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Link, entry.DateCreated.ToString(permalink.Attributes["Format"]));
							permalink.Attributes.Remove("Format");
						}
						else
						{
							permalink.Text = string.Format("<a href=\"{0}\" title = \"Permanent link to this post\">{1}</a>", entry.Link, entry.DateCreated.ToString("f"));
						}
					}

					Label postDate = e.Item.FindControl("postDate") as Label;
					if(permalink != null)
					{
						if(postDate.Attributes["Format"] != null)
						{
							postDate.Text = entry.DateCreated.ToString(postDate.Attributes["Format"]);
							postDate.Attributes.Remove("Format");
						}
						else
						{
							postDate.Text = entry.DateCreated.ToString("f");
						}
					}

					Label commentCount = e.Item.FindControl("commentCount") as Label;
					if(commentCount != null)
					{
						if(entry.AllowComments)
						{
							if(entry.FeedBackCount == 0)
							{
								commentCount.Text = string.Format(linkToComments, entry.Link, "Add Comment", "");
							}
							else if(entry.FeedBackCount == 1)
							{
								commentCount.Text = string.Format(linkToComments, entry.Link, "One Comment", "");
							}
							else if(entry.FeedBackCount > 1)
							{
								commentCount.Text = string.Format(linkToComments, entry.Link, entry.FeedBackCount, " Comments");
							}
						}
					}

					Label author = e.Item.FindControl("author") as Label;
					if(author != null)
					{
						if(entry.Author != null && entry.Author.Length > 0)
						{
							author.Text = entry.Author;
						}
					}
				}
			}
		}

		// If the user is an admin AND the the skin 
		// contains an edit Hyperlink control, this 
		// will display the edit control.
		protected virtual void DisplayEditLink(Entry entry, RepeaterItemEventArgs e)
		{
			HyperLink editLink = e.Item.FindControl("editLink") as HyperLink;
			if(editLink != null)
			{
				if(Security.IsAdmin)
				{
					editLink.Visible = true;
					if(editLink.Text.Length == 0 && editLink.ImageUrl.Length == 0)
					{
						//We'll slap on our little pencil icon.
						editLink.ImageUrl = "~/Images/edit.gif";
						editLink.Attributes["title"] = "Edit Entry";
						editLink.NavigateUrl = UrlFormats.GetEditLink(entry);
					}
				}
				else
				{
					editLink.Visible = false;
				}
			}
		}

		private EntryCollection entries;
		public EntryCollection EntryListItems
		{
			get{return entries;}
			set{entries = value;}
		}

		private bool descriptionOnly = true;
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

			if(EntryListItems != null)
			{
				EntryCollectionTitle.Text = EntryListTitle;

				if(EntryListDescription != null)
				{
					this.EntryCollectionDescription.Text = EntryListDescription;
				}
				else
				{
					EntryCollectionDescription.Visible = false;
				}

				if(EntryListReadMoreUrl != null && EntryListReadMoreText != null)
				{
					this.EntryCollectionReadMoreLink.Text = EntryListReadMoreText;
					this.EntryCollectionReadMoreLink.NavigateUrl = EntryListReadMoreUrl;
				}
				else
				{
					EntryCollectionReadMoreLink.Visible = false;
				}

				Entries.DataSource = EntryListItems;
				Entries.DataBind();
			}
		}
	}
}

