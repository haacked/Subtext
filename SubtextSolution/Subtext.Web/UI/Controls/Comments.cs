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
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Common.Data;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
using System.Configuration;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Codebehind for the control that displays comments/trackbacks/pingbacks.
	/// </summary>
	public class Comments : BaseControl
	{
		protected System.Web.UI.WebControls.Repeater CommentList;
		protected System.Web.UI.WebControls.Literal NoCommentMessage;

		private bool gravatarEnabled;
		private string gravatarUrlFormatString;
		private string gravatarEmailFormat;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			try
			{
                gravatarEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["GravatarEnabled"]);
			}
			catch(Exception) 
			{
				gravatarEnabled = false;
			}
			
			if(gravatarEnabled) 
			{
                gravatarUrlFormatString = ConfigurationManager.AppSettings["GravatarUrlFormatString"];
                gravatarEmailFormat = ConfigurationManager.AppSettings["GravatarEmailFormat"];
			}

			if(CurrentBlog.CommentsEnabled)
			{
				Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);	

				if(entry != null && entry.AllowComments)
				{
					BindComments(entry);
				}
				else
				{
					this.Visible = false;
				}
			}
			else
			{
				this.Visible = false;
			}
			
		}


		protected void RemoveComment_ItemCommand(Object Sender, RepeaterCommandEventArgs e) 
		{
			int feedbackItem = Int32.Parse(e.CommandName);
			Entries.Delete(feedbackItem);
			Response.Redirect(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?Pending=true", Request.Path));
		}

		protected void CommentsCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				if(entry != null)
				{
					Literal title = (Literal)(e.Item.FindControl("Title"));
					if(title != null)
					{
						// we should probably change skin format to dynamically wire up to 
						// skin located title and permalinks at some point
						title.Text = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{2}&nbsp;{0}{1}", Anchor(entry.Id), 
							entry.Title, Link(entry.Title, entry.Url));
					}

					HyperLink namelink = (HyperLink)e.Item.FindControl("NameLink");
					if(namelink != null)
					{
						if(entry.HasAlternativeTitleUrl)
						{
							namelink.NavigateUrl = HtmlHelper.CheckForUrl(entry.TitleUrl);
						}
						if(entry.PostType == PostType.Comment)
						{
							namelink.Text = entry.Author;
						}
						else if(entry.PostType == PostType.PingTrack)
						{
							namelink.Text =  entry.Author != null ? entry.Author : "Pingback/TrackBack";
							namelink.Attributes.Add("title", "PingBack/TrackBack");
						}
						ControlHelper.SetTitleIfNone(namelink, entry.SourceUrl);
					}

					Literal PostDate = (Literal)(e.Item.FindControl("PostDate"));
					if(PostDate != null)
					{
						PostDate.Text = entry.DateCreated.ToShortDateString() + " " + entry.DateCreated.ToShortTimeString();
					}

					Literal Post = e.Item.FindControl("PostText") as Literal;
					if(Post != null)
					{
						if(entry.Body.Length > 0)
						{
							Post.Text = entry.Body;
							if(entry.Body.Length == 0 && entry.PostType == PostType.PingTrack)
							{
								Post.Text = "Pingback / Trackback";
							}
						}
					}
					
					if(gravatarEnabled)
					{
						System.Web.UI.WebControls.Image Gravatar = e.Item.FindControl("GravatarImg") as System.Web.UI.WebControls.Image;
						if(Gravatar != null) 
						{
							string gravatarUrl = Gravatar.Attributes["PlaceHolderImage"];
							
							if (!String.IsNullOrEmpty(entry.Email))
                                gravatarUrl = BuildGravatarUrl(entry.Email, gravatarUrl);
							
							if(!String.IsNullOrEmpty(gravatarUrl))
							{
								if(gravatarUrl.Length != 0)
								{
									Gravatar.ImageUrl = gravatarUrl;
									Gravatar.Visible = true;
								}
							}
						}
					}

					if(Request.IsAuthenticated && Security.IsAdmin)
					{
						LinkButton editlink = (LinkButton)(e.Item.FindControl("EditLink"));
						if(editlink != null)
						{
							//editlink.CommandName = "Remove";
							editlink.Text = "Remove Comment " + entry.Id.ToString(CultureInfo.InvariantCulture);
							editlink.CommandName = entry.Id.ToString(CultureInfo.InvariantCulture);
							editlink.Attributes.Add("onclick","return confirm(\"Are you sure you want to delete comment " + entry.Id.ToString(CultureInfo.InvariantCulture) + "?\");");
							editlink.Visible = true;
							editlink.CommandArgument = entry.Id.ToString(CultureInfo.InvariantCulture);

							ControlHelper.SetTitleIfNone(editlink, "Click to remove this entry.");
						}
						else
						{
							editlink.Visible = false;
						}
					}
				}
			}
		}

		const string linktag = "<a title=\"permalink: {0}\" href=\"{1}\">#</a>";
		private string Link(string title, string link)
		{
			return string.Format(linktag,title,link);
		}

		// GC: xhmtl format wreaking havoc in non-xhtml pages in non-IE, changed to non nullable format
		const string anchortag = "<a name=\"{0}\"></a>";
		private string Anchor(int id)
		{
			return string.Format(anchortag, id);
		}

		private string BuildGravatarUrl(string email,string defaultGravatar) 
		{
			string processedEmail = string.Empty;

            defaultGravatar = CurrentBlog.RootUrl.TrimEnd('/') + ControlHelper.ExpandTildePath(defaultGravatar);

			if(gravatarEmailFormat.Equals("plain"))
			{
				processedEmail = email;
			}
			else if(gravatarEmailFormat.Equals("MD5")) 
			{
				processedEmail=System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(email, "md5");
			}
			if(processedEmail.Length != 0)
                return String.Format(gravatarUrlFormatString, processedEmail, defaultGravatar);
			else
				return string.Empty;
		}

		void BindComments(Entry entry)
		{
				try
				{
					if(Request.QueryString["Pending"] != null)
					{
						Cacher.ClearCommentCache(entry.Id);
					}
					CommentList.DataSource = Cacher.GetComments(entry, CacheDuration.Short);
					CommentList.DataBind();

					if(CommentList.Items.Count == 0)
					{
						if (entry.CommentingClosed)
						{
							this.Controls.Clear();
						}
						else
						{
							CommentList.Visible = false;
							this.NoCommentMessage.Text = "No comments posted yet.";
						}
					}

				}
				catch
				{
					this.Visible = false;
				}
		}
	}
}

