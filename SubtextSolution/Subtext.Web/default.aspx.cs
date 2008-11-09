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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public partial class _default : Page
	{
		protected HyperLink Hyperlink6;
		protected HyperLink Hyperlink7;

		protected void Page_Load(object sender, EventArgs e)
		{
            //No postbacks on this page. It is output cached.
			SetStyle();
            DataBind();
		}

		private string aggregateUrl = null;
		/// <summary>
		/// Url to the aggregate page.
		/// </summary>
		protected string AggregateUrl
		{
			get
			{
				if (this.aggregateUrl == null)
				{
					this.aggregateUrl = ConfigurationManager.AppSettings["AggregateUrl"];
					if (Request.Url.Port != 80)
					{
						UriBuilder url = new UriBuilder(aggregateUrl);
						url.Port = Request.Url.Port;
						this.aggregateUrl = url.ToString();
					}
				}
				return aggregateUrl;
			}
		}

		private void SetStyle()
		{
			const string style = "<link href=\"{0}{1}\" type=\"text/css\" rel=\"stylesheet\">";
			string apppath = HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? HttpContext.Current.Request.ApplicationPath : HttpContext.Current.Request.ApplicationPath + "/";
            //TODO: This is hard-coded to look in the simple skin for aggregate blogs. We should change this later.
            Style.Text = string.Format(style, apppath, "Skins/Aggregate/Simple/Style.css") + "\n" + string.Format(style, apppath, "Skins/Aggregate/Simple/blue.css") + "\n" + string.Format(style, apppath, "Scripts/Lightbox.css");
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}



