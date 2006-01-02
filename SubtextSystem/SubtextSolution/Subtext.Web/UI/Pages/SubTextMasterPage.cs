using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Web.UI.Skinning;

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
		private static readonly ScriptElementCollectionRenderer __scriptRenderer = new ScriptElementCollectionRenderer();
		private static readonly StyleSheetElementCollectionRenderer __styleRenderer = new StyleSheetElementCollectionRenderer();
		protected System.Web.UI.WebControls.Literal pageTitle;
		protected System.Web.UI.WebControls.Literal docTypeDeclaration;
		protected System.Web.UI.HtmlControls.HtmlGenericControl MainStyle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl SecondaryCss;
		protected System.Web.UI.HtmlControls.HtmlGenericControl RSSLink;
		protected System.Web.UI.WebControls.PlaceHolder CenterBodyControl;
		protected System.Web.UI.WebControls.Literal authorMetaTag;
		protected System.Web.UI.WebControls.Literal scripts;
		protected System.Web.UI.WebControls.Literal styles;
		#endregion
		
		protected BlogInfo CurrentBlog;
		protected const string TemplateLocation = "~/Skins/{0}/{1}";
		protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";

		private void InitializeBlogPage()
		{
			CurrentBlog = Config.CurrentBlog;
			string skin = Globals.Skin(Context);

			SpecifyDocType();

			string[] controls = Subtext.Common.UrlManager.HandlerConfiguration.GetControls(Context);
			foreach(string control in controls)
			{
				Control c = LoadControl(string.Format(ControlLocation, skin, control));
				c.ID = control;
				CenterBodyControl.Controls.Add(c);
			}

			string path = (Context.Request.ApplicationPath + "/skins/" + skin + "/").Replace("//","/");

			MainStyle.Attributes.Add("href", path + "style.css");

			if(CurrentBlog.Skin.HasSecondaryFile)
			{
				SecondaryCss.Attributes.Add("href", path + CurrentBlog.Skin.SkinCssFile);
			}
			else if(CurrentBlog.Skin.HasSecondaryText)
			{
				SecondaryCss.Attributes.Add("href", CurrentBlog.RootUrl  + "customcss.aspx");
			}
			else
			{
				//MAC IE does not like the empy CSS file..plus its a waste :)
				SecondaryCss.Visible = false;

			}
			RSSLink.Attributes.Add("href", CurrentBlog.RootUrl + "rss.aspx");

			// if specified, add script elements
			if (scripts != null)
			{
				scripts.Text = __scriptRenderer.RenderScriptElementCollection(Context, skin);
			}

			if(styles != null)
			{
				styles.Text = __styleRenderer.RenderStyleElementCollection(Context, skin);
			}
		}

		//	Renders the DocType tag and specifies an xmlns for the HTML 
		//	tag if using XHTML.
		private void SpecifyDocType()
		{
			if(docTypeDeclaration != null)
			{
				docTypeDeclaration.Text = string.Empty;
				if(Config.Settings.DocTypeDeclaration != null && Config.Settings.DocTypeDeclaration.Length > 0)
				{

					// DG: Removed as this forces IE to go into the Quirks mode
//					if(Config.Settings.UseXHTML)
//					{
//						docTypeDeclaration.Text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine;
//					}
					docTypeDeclaration.Text += "<" + Config.Settings.DocTypeDeclaration + ">" + Environment.NewLine;
					docTypeDeclaration.Text += "<html";
					
					if(Config.Settings.UseXHTML)
					{
						docTypeDeclaration.Text += " xmlns=\"http://www.w3.org/1999/xhtml\"";
						docTypeDeclaration.Text += String.Format(" lang=\"{0}\"", Config.CurrentBlog.LanguageCode);
						docTypeDeclaration.Text += String.Format(" xml:lang=\"{0}\"", Config.CurrentBlog.LanguageCode);
					}
					docTypeDeclaration.Text += ">";
				}
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
		private class ScriptElementCollectionRenderer
		{
			private string RenderScriptAttribute(string attributeName, string attributeValue)
			{
                return attributeValue != null ? " " + attributeName + "=\"" + attributeValue + "\"" : String.Empty;
			}

			private string RenderScriptElement(string skinPath, Script script)
			{
				return "<script" + 
					RenderScriptAttribute("type", script.Type) + 
					RenderScriptAttribute("src", skinPath + script.Src) + 
					"></script>\n";
			}

			private string CreateSkinPath(HttpContext httpContext, string skinName)
			{
				string applicationPath = httpContext.Request.ApplicationPath;
				return (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinName + "/";
			}

			public string RenderScriptElementCollection(HttpContext httpContext, string skinName)
			{
				string result = String.Empty;

				SkinTemplates skinTemplates = SkinTemplates.Instance(httpContext);
				SkinTemplate skinTemplate = skinTemplates.GetTemplate(skinName);
				if (skinTemplate != null && skinTemplate.Scripts != null)
				{
					string skinPath = CreateSkinPath(httpContext, skinName);
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
		private class StyleSheetElementCollectionRenderer
		{
			private string RenderStyleAttribute(string attributeName, string attributeValue)
			{
				return attributeValue != null ? " " + attributeName + "=\"" + attributeValue + "\"" : String.Empty;
			}

			private string RenderStyleElement(string skinPath, Style style)
			{
				string element = "<link";
				if(style.Media != null && style.Media.Length > 0)
					element += RenderStyleAttribute("media", style.Media);
 
				return element +
					RenderStyleAttribute("type", "text/css") + 
					RenderStyleAttribute("rel", "stylesheet") + 
					RenderStyleAttribute("title", style.Title) + 
					RenderStyleAttribute("href", Path.Combine(skinPath, style.Href)) + //TODO: Look at this line again.
					"></link>" + Environment.NewLine;
			}

			private string CreateStylePath(HttpContext httpContext, string skinName)
			{
				string applicationPath = httpContext.Request.ApplicationPath;
				string path = (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinName + "/";
				return path;
			}

			public string RenderStyleElementCollection(HttpContext httpContext, string skinName)
			{
				string result = String.Empty;

				SkinTemplates skinTemplates = SkinTemplates.Instance(httpContext);
				SkinTemplate skinTemplate = skinTemplates.GetTemplate(skinName);
				
				if (skinTemplate != null && skinTemplate.Styles != null)
				{
					string skinPath = CreateStylePath(httpContext, skinName);
					foreach(Style style in skinTemplate.Styles)
					{
						result += RenderStyleElement(skinPath, style);
					}
				}
				return result;
			}
		}

	}
}
