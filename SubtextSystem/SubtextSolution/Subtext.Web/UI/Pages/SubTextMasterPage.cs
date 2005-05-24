using System;
using System.Web.UI;
using Subtext.Framework.Configuration;

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
		protected System.Web.UI.WebControls.Literal pageTitle;
		protected System.Web.UI.WebControls.Literal docTypeDeclaration;
		protected System.Web.UI.HtmlControls.HtmlGenericControl MainStyle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl SecondaryCss;
		protected System.Web.UI.HtmlControls.HtmlGenericControl RSSLink;
		protected System.Web.UI.WebControls.PlaceHolder CenterBodyControl;
		
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
				SecondaryCss.Attributes.Add("href", CurrentBlog.FullyQualifiedUrl  + "customcss.aspx");
			}
			else
			{
				//MAC IE does not like the empy CSS file..plus its a waste :)
				SecondaryCss.Visible = false;

			}
			RSSLink.Attributes.Add("href", CurrentBlog.FullyQualifiedUrl + "rss.aspx");
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
					if(Config.Settings.UseXHTML)
					{
						docTypeDeclaration.Text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine;
					}
					docTypeDeclaration.Text += "<" + Config.Settings.DocTypeDeclaration + ">" + Environment.NewLine;
					docTypeDeclaration.Text += "<html";
					
					if(Config.Settings.UseXHTML)
					{
						docTypeDeclaration.Text += " xmlns=\"http://www.w3.org/1999/xhtml\"";
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
			//Is this for extra security?
			this.EnableViewState = false;
			pageTitle.Text = Globals.CurrentTitle(Context);
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

	}
}
