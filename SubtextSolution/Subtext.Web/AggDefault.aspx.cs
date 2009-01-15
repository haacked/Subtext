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
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Web.UI.Controls.Aggregate;

namespace Subtext.Web
{
    /// <summary>
    /// When AggregateBlog is enabled in Web.config, this page renders contents from 
    /// all blogs which have set their blog posts to be included in the main blog.
    /// </summary>
	public partial class AggDefault : AggregatePage
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



