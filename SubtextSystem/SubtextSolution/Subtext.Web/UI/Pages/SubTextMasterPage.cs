using System;
using System.Web.UI;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Pages
{
	/// <summary>
	/// Summary description for DottextMasterPage.
	/// </summary>
	public class SubtextMasterPage : ViewStatelessPage
	{

		public SubtextMasterPage()
		{
		}

		protected System.Web.UI.WebControls.Literal pageTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl MainStyle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl SecondaryCss;
		protected System.Web.UI.HtmlControls.HtmlGenericControl RSSLink;
		protected System.Web.UI.WebControls.PlaceHolder CenterBodyControl;
		
		protected BlogConfig CurrentBlog;
		protected const string TemplateLocation = "~/Skins/{0}/{1}";
		protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";

		private void InitializeBlogPage()
		{
			CurrentBlog = Config.CurrentBlog(Context);
			string skin = Globals.Skin(Context);

			string[] controls = Subtext.Common.UrlManager.HandlerConfiguration.GetControls(Context);
			foreach(string control in controls)
			{
				Control c = LoadControl(string.Format(ControlLocation,skin,control));
				c.ID = control;
				CenterBodyControl.Controls.Add(c);
			}

			
			string path = (Context.Request.ApplicationPath + "/skins/" + skin + "/").Replace("//","/");

			MainStyle.Attributes.Add("href", path+"style.css");

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
			RSSLink.Attributes.Add("href",CurrentBlog.FullyQualifiedUrl+"rss.aspx");
		}


		protected override void OnPreRender(EventArgs e)
		{
			this.EnableViewState = false;
			pageTitle.Text = Globals.CurrentTitle(Context);
			base.OnPreRender (e);
		}

		override protected void OnInit(EventArgs e)
		{
			InitializeBlogPage();
			base.OnInit(e);
		}

	}
}
