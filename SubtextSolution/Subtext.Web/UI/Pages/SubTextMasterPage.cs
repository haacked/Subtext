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
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.UrlManager;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.UI.Pages
{
	/// <summary>
	/// This serves as the master page for every page within the 
	/// site (except for the admin section.  This page contains 
	/// a PlaceHolder in which the PageTemplate.ascx control within 
	/// each skin is loaded.
	/// </summary>
	public class SubtextMasterPage : Page
	{
		#region Declared Controls in DTP.aspx
		private static readonly ScriptElementCollectionRenderer scriptRenderer = new ScriptElementCollectionRenderer(SkinTemplates.Instance());
		private static readonly StyleSheetElementCollectionRenderer styleRenderer = new StyleSheetElementCollectionRenderer(SkinTemplates.Instance());
		protected Literal pageTitle;
		protected Literal docTypeDeclaration;
		protected HtmlLink CustomCss;
		protected HtmlLink RSSLink;
		protected HtmlLink Rsd;
		protected HtmlLink AtomLink;
		protected PlaceHolder CenterBodyControl;
		protected Literal versionMetaTag;
		protected Literal authorMetaTag;
		protected Literal scriptsOnTop;
        protected Literal scriptsOnBottom;
		protected Literal styles;
		protected Literal virtualRoot;
		protected Literal virtualBlogRoot;
		protected Literal customTrackingCode;
		protected Literal additionalMetaTags;
        protected PlaceHolder scriptsOnTopCont;
        protected PlaceHolder scriptsOnBottomCont;
		#endregion

		protected BlogInfo CurrentBlog;
		protected Comments commentsControl;
		protected PostComment postCommentControl;
		protected const string TemplateLocation = "~/Skins/{0}/{1}";
		protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";

		private void InitializeBlogPage()
		{
			MaintainScrollPositionOnPostBack = true;
			CurrentBlog = Config.CurrentBlog;

			string skinFolder = CurrentBlog.Skin.TemplateFolder;

			string[] controls = HandlerConfiguration.GetControls(Context);
			if (controls != null)
			{
				UpdatePanel apnlCommentsWrapper = new UpdatePanel();
				apnlCommentsWrapper.Visible = true;
				apnlCommentsWrapper.ID = "apnlCommentsWrapper";

				foreach (string controlId in controls)
				{
					Control control = LoadControl(string.Format(ControlLocation, skinFolder, controlId));
					control.ID = controlId.Replace(".", "_");

					if (controlId.Equals("Comments.ascx"))
					{
						control.Visible = true;
						commentsControl = control as Comments;
						apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
					}
					else if (controlId.Equals("PostComment.ascx"))
					{
						postCommentControl = (PostComment)control;
						postCommentControl.CommentApproved += postCommentControl_CommentPosted;
						apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
						CenterBodyControl.Controls.Add(apnlCommentsWrapper);
					}
					else
					{
						CenterBodyControl.Controls.Add(control);
					}
				}
			}

			if(CurrentBlog.Skin.HasCustomCssText)
			{
				CustomCss.Attributes.Add("href", CurrentBlog.RootUrl + "customcss.aspx");
			}
			else
			{
				//MAC IE does not like the empy CSS file, plus its a waste :)
				CustomCss.Visible = false;
			}

			if (Rsd != null)
			{
				Rsd.Attributes.Add("href", CurrentBlog.RootUrl + "rsd.xml.ashx");
			}

			if (RSSLink != null)
			{
				RSSLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
			}

			if (AtomLink != null)
			{
				AtomLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
			}

            Literal scripts = scriptsOnTop;
		    ScriptMergeMode mode = scriptRenderer.GetScriptMergeMode(CurrentBlog.Skin.SkinKey);

            if(mode == ScriptMergeMode.None || mode == ScriptMergeMode.MergeOnTop)
            {
                scripts = scriptsOnTop;
                scriptsOnTopCont.Visible = true;
                scriptsOnBottomCont.Visible = false;
            }
            else if (mode == ScriptMergeMode.MergeOnBottom)
            {
                scripts = scriptsOnBottom;
                scriptsOnTopCont.Visible = false;
                scriptsOnBottomCont.Visible = true;
            }

		    // if specified, add script elements
			if (scripts != null)
			{
				scripts.Text = scriptRenderer.RenderScriptElementCollection(CurrentBlog.Skin.SkinKey);
			}

			if (styles != null)
			{
                styles.Text = styleRenderer.RenderStyleElementCollection(CurrentBlog.Skin.SkinKey);
			}

			// Add the per-blog MetaTags to the page Head section.
			ICollection<MetaTag> blogMetaTags = MetaTags.GetMetaTagsForBlog(CurrentBlog);
			foreach (MetaTag tag in blogMetaTags)
			{
				HtmlMeta mt = new HtmlMeta();
				mt.Content = tag.Content;

				if (!string.IsNullOrEmpty(tag.Name))
					mt.Name = tag.Name;
				else
					mt.HttpEquiv = tag.HttpEquiv;

				Page.Header.Controls.Add(mt);
			}
		}

		void postCommentControl_CommentPosted(object sender, EventArgs e)
		{
			commentsControl.BindFeedback(false); //don't get it from cache.
		}

		/// <summary>
		/// Before rendering, turns off ViewState again (why? not sure),  
		/// applies skins and stylesheet references, and sets the page title.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnPreRender(EventArgs e)
		{
			Response.ContentEncoding = Encoding.UTF8; //TODO: allow for per/blog config.
			Response.ContentType = "text/html"; //TODO: allow for per/blog config.

			//Is this for extra security?
			EnableViewState = false;
			pageTitle.Text = Globals.CurrentTitle(Context);
			if (!String.IsNullOrEmpty(Config.CurrentBlog.Author))
			{
				authorMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"author\" content=\"{0}\" />", Config.CurrentBlog.Author);
			}
			versionMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"Generator\" content=\"{0}\" />", VersionInfo.VersionDisplayText);

			if (!String.IsNullOrEmpty(Config.CurrentBlog.CustomMetaTags))
			{
				additionalMetaTags.Text = Environment.NewLine + Config.CurrentBlog.CustomMetaTags + Environment.NewLine;
			}

			if (!String.IsNullOrEmpty(Config.CurrentBlog.TrackingCode))
			{
				customTrackingCode.Text = Config.CurrentBlog.TrackingCode;
			}

			base.OnPreRender(e);

		}

		/// <summary>
		/// Initializes this blog page, applying skins and whatnot.
		/// </summary>
		/// <param name="e">E.</param>
		override protected void OnInit(EventArgs e)
		{
			InitializeBlogPage();
			base.OnInit(e);
		}

		/// <summary>
		/// Loads the page state from persistence medium.  In this case 
		/// this returns null as we are not using ViewState.
		/// </summary>
		/// <returns></returns>
		protected override object LoadPageStateFromPersistenceMedium()
		{
			return null;
		}

		/// <summary>
		/// Saves the page state to persistence medium.  In this case 
		/// this does nothing as we are not using ViewState.
		/// </summary>
		/// <param name="viewState">State of the view.</param>
		protected override void SavePageStateToPersistenceMedium(object viewState) { }
	}
}
