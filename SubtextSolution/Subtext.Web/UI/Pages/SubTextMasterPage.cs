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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.UrlManager;
using Subtext.Web.Controls;
using Subtext.Web.UI.Controls;
using Style=Subtext.Framework.UI.Skinning.Style;

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
		protected HtmlLink MainStyle;
		protected HtmlLink SecondaryCss;
		protected HtmlLink CustomCss;
		protected HtmlLink RSSLink;
		protected HtmlLink Rsd;
		protected HtmlLink AtomLink;
		protected PlaceHolder CenterBodyControl;
		protected Literal versionMetaTag;
        protected Literal authorMetaTag;
		protected Literal scripts;
		protected Literal styles;
		protected Literal virtualRoot;
		protected Literal virtualBlogRoot;
		protected Literal customTrackingCode;
		protected Literal additionalMetaTags;
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

			string skinFolder = Config.CurrentBlog.Skin.TemplateFolder;

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

			string path = (HttpContext.Current.Request.ApplicationPath + "/skins/" + skinFolder + "/").Replace("//","/");

			MainStyle.Attributes.Add("href", path + "style.css");

			if(CurrentBlog.Skin.HasStyleSheet)
			{
				SecondaryCss.Attributes.Add("href", path + CurrentBlog.Skin.SkinStyleSheet);
			}
			else
			{
				SecondaryCss.Visible = false;
			}
			
			if(CurrentBlog.Skin.HasCustomCssText)
			{
				CustomCss.Attributes.Add("href", CurrentBlog.RootUrl + "customcss.aspx");
			}
			else
			{
				//MAC IE does not like the empy CSS file..plus its a waste :)
				CustomCss.Visible = false;
			}
			
			if(Rsd != null)
			{
				Rsd.Attributes.Add("href", CurrentBlog.RootUrl + "rsd.xml.ashx");
			}
			
			if(RSSLink != null)
			{
				RSSLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
			}
			
			if(AtomLink != null)
			{
				AtomLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
			}

			// if specified, add script elements
			if (scripts != null)
			{
				scripts.Text = scriptRenderer.RenderScriptElementCollection(Config.CurrentBlog.Skin.SkinKey);
			}

			if(styles != null)
			{
				styles.Text = styleRenderer.RenderStyleElementCollection(Config.CurrentBlog.Skin.SkinKey);
			}

            // Add the per-blog MetaTags to the page Head section.
            ICollection<MetaTag> blogMetaTags = MetaTags.GetMetaTagsForBlog(Config.CurrentBlog);
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

            base.OnPreRender (e);

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
		protected override void SavePageStateToPersistenceMedium(object viewState){}

		#region private class ScriptElementCollectionRenderer

		/// <summary>
		/// Provides rendering facilities for script elements in the head element of the page
		/// </summary>
		public class ScriptElementCollectionRenderer
		{
			SkinTemplates templates;
			
			public ScriptElementCollectionRenderer(SkinTemplates templates)
			{
				this.templates = templates;
			}
			
			private static string RenderScriptAttribute(string attributeName, string attributeValue)
			{
                return attributeValue != null ? " " + attributeName + "=\"" + attributeValue + "\"" : String.Empty;
			}

			public static string RenderScriptElement(string skinPath, Script script)
			{
				return "<script" +
				       RenderScriptAttribute("type", script.Type) +
				       RenderScriptAttribute("src", GetScriptSourcePath(skinPath, script)) +
				       RenderScriptAttribute("defer", script.Defer ? "defer" : null) +
					"></script>" + Environment.NewLine;
			}

			private static string GetScriptSourcePath(string skinPath, Script script)
			{
				if(script.Src.StartsWith("~"))
				{
					return ControlHelper.ExpandTildePath(script.Src);
				}
				else if(script.Src.StartsWith("/"))
				{
					return script.Src;
				}
				else
				{
					return skinPath + script.Src;
				}
			}

			/// <summary>
			/// Gets the skin path.
			/// </summary>
			/// <param name="skinTemplateFolder">Name of the skin.</param>
			/// <returns></returns>
			private static string GetSkinPath(string skinTemplateFolder)
			{
				string applicationPath = HttpContext.Current.Request.ApplicationPath;
				return (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinTemplateFolder + "/";
			}

			/// <summary>
			/// Renders the script element collection for thes kin key.
			/// </summary>
			/// <param name="skinKey">The skin key.</param>
			/// <returns></returns>
			public string RenderScriptElementCollection(string skinKey)
			{
				StringBuilder result = new StringBuilder();

				SkinTemplate skinTemplate = templates.GetTemplate(skinKey);
				if (skinTemplate != null && skinTemplate.Scripts != null)
				{
					string skinPath = GetSkinPath(skinTemplate.TemplateFolder);
					foreach(Script script in skinTemplate.Scripts)
					{
						result.Append(RenderScriptElement(skinPath, script));
					}
				}
				return result.ToString();
			}
		}

		#endregion

		/// <summary>
		/// Provides rendering facilities for stylesheet elements in the head element of the page
		/// </summary>
		public class StyleSheetElementCollectionRenderer
		{
			SkinTemplates templates;
			
			public StyleSheetElementCollectionRenderer(SkinTemplates templates)
			{
				this.templates = templates;
			}
			
			private static string RenderStyleAttribute(string attributeName, string attributeValue)
			{
				return attributeValue != null ? " " + attributeName + "=\"" + attributeValue + "\"" : String.Empty;
			}

			private static string RenderStyleElement(string skinPath, Style style)
			{
                StringBuilder element = new StringBuilder();
			    
                if (!String.IsNullOrEmpty(style.Conditional))
                {
                    element.Append(string.Format("<!--[{0}]>{1}", style.Conditional, Environment.NewLine));
                }

                element.Append("<link");
                if (style.Media != null && style.Media.Length > 0)
                {
                    element.Append(RenderStyleAttribute("media", style.Media));
                }

                element.Append(RenderStyleAttribute("type", "text/css") + 
					RenderStyleAttribute("rel", "stylesheet") + 
					RenderStyleAttribute("title", style.Title) + 
					RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style)) + //TODO: Look at this line again.
					"></link>" + Environment.NewLine);

                if (!String.IsNullOrEmpty(style.Conditional))
                {
                    element.Append("<![endif]-->" + Environment.NewLine);
                }
			    
			    return element.ToString();
			}

			/// <summary>
			/// Gets the stylesheet href path.
			/// </summary>
			/// <param name="skinPath">The skin path.</param>
			/// <param name="style">The style.</param>
			/// <returns></returns>
			public static string GetStylesheetHrefPath(string skinPath, Style style)
			{
				if(style.Href.StartsWith("~"))
				{
					return ControlHelper.ExpandTildePath(style.Href);
				}
				else if(style.Href.StartsWith("/") || style.Href.StartsWith("http://") || style.Href.StartsWith("https://"))
				{
					return style.Href;
				}
				else
				{
					return skinPath + style.Href;
				}
			}

			private static string CreateStylePath(string skinTemplateFolder)
			{
				string applicationPath = HttpContext.Current.Request.ApplicationPath;
				string path = (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinTemplateFolder + "/";
				return path;
			}

			public string RenderStyleElementCollection(string skinName)
			{
				StringBuilder result = new StringBuilder();

				SkinTemplate skinTemplate = templates.GetTemplate(skinName);
				
				if (skinTemplate != null && skinTemplate.Styles != null)
				{
					string skinPath = CreateStylePath(skinTemplate.TemplateFolder);
					foreach(Style style in skinTemplate.Styles)
					{
						result.Append(RenderStyleElement(skinPath, style));
					}
				}
				return Environment.NewLine + result + Environment.NewLine;
			}
		}

		/// <summary>
		/// Returns the text for a javascript array of allowed elements. 
		/// This will be used by other scripts.
		/// </summary>
		/// <value>The allowed HTML javascript declaration.</value>
		protected static string AllowedHtmlJavascriptDeclaration
		{
			get
			{
				string declaration = "var subtextAllowedHtmlTags = [";
				for(int i = 0; i < Config.Settings.AllowedHtmlTags.Count; i++)
				{
					string tagname = Config.Settings.AllowedHtmlTags.Keys[i];
					declaration += string.Format("'{0}', ", tagname);
				}
				if(Config.Settings.AllowedHtmlTags.Count > 0)
				{
					declaration = StringHelper.Left(declaration, declaration.Length - 2);
				}

				return declaration + "];";
			}
		}
	}
}
