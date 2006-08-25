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
using System.Text;
using System.Web;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Pages
{
	/// <summary>
	/// This serves as the master page for every page within the 
	/// site (except for the admin section.  This page contains 
	/// a PlaceHolder in which the PageTemplate.ascx control within 
	/// each skin is loaded.
	/// </summary>
	public class SubtextMasterPage : System.Web.UI.Page
	{
		#region Declared Controls in DTP.aspx
		private static readonly ScriptElementCollectionRenderer scriptRenderer = new ScriptElementCollectionRenderer(SkinTemplates.Instance());
		private static readonly StyleSheetElementCollectionRenderer styleRenderer = new StyleSheetElementCollectionRenderer(SkinTemplates.Instance());
		protected System.Web.UI.WebControls.Literal pageTitle;
		protected System.Web.UI.WebControls.Literal docTypeDeclaration;
		protected System.Web.UI.HtmlControls.HtmlLink MainStyle;
		protected System.Web.UI.HtmlControls.HtmlLink SecondaryCss;
		protected System.Web.UI.HtmlControls.HtmlLink CustomCss;
		protected System.Web.UI.HtmlControls.HtmlLink RSSLink;
		protected System.Web.UI.HtmlControls.HtmlLink Rsd;
		protected System.Web.UI.HtmlControls.HtmlLink AtomLink;
		protected System.Web.UI.WebControls.PlaceHolder CenterBodyControl;
		protected System.Web.UI.WebControls.Literal authorMetaTag;
		protected System.Web.UI.WebControls.Literal scripts;
		protected System.Web.UI.WebControls.Literal styles;
		protected System.Web.UI.WebControls.Literal virtualRoot;
		protected System.Web.UI.WebControls.Literal virtualBlogRoot;
		#endregion
		
		protected BlogInfo CurrentBlog;
		protected const string TemplateLocation = "~/Skins/{0}/{1}";
		protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";

		private void InitializeBlogPage()
		{
			CurrentBlog = Config.CurrentBlog;

			string skinFolder = Config.CurrentBlog.Skin.TemplateFolder;

			string[] controls = Subtext.Framework.UrlManager.HandlerConfiguration.GetControls(Context);
            if (controls != null)
            {
                foreach (string control in controls)
                {
                    Control c = LoadControl(string.Format(ControlLocation, skinFolder, control));
                    c.ID = control.Replace(".", "_");
                    CenterBodyControl.Controls.Add(c);
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
				RSSLink.Attributes.Add("href", CurrentBlog.RootUrl + "rss.aspx");
			}
			
			if(AtomLink != null)
			{
				AtomLink.Attributes.Add("href", CurrentBlog.RootUrl + "atom.aspx");
			}

			// if specified, add script elements
			if (scripts != null)
			{
				scripts.Text = scriptRenderer.RenderScriptElementCollection(skinFolder);
			}

			if(styles != null)
			{
				styles.Text = styleRenderer.RenderStyleElementCollection(Config.CurrentBlog.Skin.SkinKey);
			}
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
			this.EnableViewState = false;
			pageTitle.Text = Globals.CurrentTitle(Context);
			if(Config.CurrentBlog.Author != null && Config.CurrentBlog.Author.Length > 0)
				authorMetaTag.Text = String.Format("<meta name=\"author\" content=\"{0}\" />", Config.CurrentBlog.Author );
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

			private static string GetSkinPath(string skinName)
			{
				string applicationPath = HttpContext.Current.Request.ApplicationPath;
				return (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinName + "/";
			}

			public string RenderScriptElementCollection(string skinKey)
			{
				string result = String.Empty;

				SkinTemplate skinTemplate = this.templates.GetTemplate(skinKey);
				if (skinTemplate != null && skinTemplate.Scripts != null)
				{
					string skinPath = GetSkinPath(skinTemplate.TemplateFolder);
					foreach(Script script in skinTemplate.Scripts)
					{
						result += RenderScriptElement(skinPath, script);
					}
				}
				return result;
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
                string element = string.Empty;
			    
                if (!String.IsNullOrEmpty(style.Conditional))
                {
                    element = string.Format("<!--[{0}]>{1}", style.Conditional, Environment.NewLine);
                }
			    
                element += "<link";
                    if (style.Media != null && style.Media.Length > 0)
                        element += RenderStyleAttribute("media", style.Media);

				element +=
					RenderStyleAttribute("type", "text/css") + 
					RenderStyleAttribute("rel", "stylesheet") + 
					RenderStyleAttribute("title", style.Title) + 
					RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style)) + //TODO: Look at this line again.
					"></link>" + Environment.NewLine;

                if (!String.IsNullOrEmpty(style.Conditional))
                {
                    element += "<![endif]-->" + Environment.NewLine;
                }
			    
			    return element;
			}

			public static string GetStylesheetHrefPath(string skinPath, Style style)
			{
				if(style.Href.StartsWith("~"))
				{
					return ControlHelper.ExpandTildePath(style.Href);
				}
				else if(style.Href.StartsWith("/"))
				{
					return style.Href;
				}
				else
				{
					return skinPath + style.Href;
				}
			}

			private string CreateStylePath(string skinTemplateFolder)
			{
				string applicationPath = HttpContext.Current.Request.ApplicationPath;
				string path = (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinTemplateFolder + "/";
				return path;
			}

			public string RenderStyleElementCollection(string skinName)
			{
				string result = String.Empty;

				SkinTemplate skinTemplate = this.templates.GetTemplate(skinName);
				
				if (skinTemplate != null && skinTemplate.Styles != null)
				{
					string skinPath = CreateStylePath(skinTemplate.TemplateFolder);
					foreach(Style style in skinTemplate.Styles)
					{
						result += RenderStyleElement(skinPath, style);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Returns the text for a javascript array of allowed elements. 
		/// This will be used by other scripts.
		/// </summary>
		/// <value>The allowed HTML javascript declaration.</value>
		protected string AllowedHtmlJavascriptDeclaration
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
