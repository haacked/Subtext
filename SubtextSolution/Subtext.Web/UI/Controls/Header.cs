using System;
using Subtext.Web.Controls;

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

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Used to display the header.
	/// </summary>
	public class Header : BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink HeaderTitle;
		protected System.Web.UI.WebControls.Literal HeaderSubTitle;
	
		protected string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}
		private string title;

		public string Subtitle
		{
			get { return this.subtitle; }
			set { this.subtitle = value; }
		}
		private string subtitle;

		protected string HomeUrl
		{
			get { return this.homeUrl; }
			set { this.homeUrl = value; }
		}
		private string homeUrl;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			this.title = Blog.Title;
			this.homeUrl = Blog.HomeVirtualUrl;
			this.subtitle = Blog.SubTitle;

			if (null != this.FindControl("HeaderTitle"))
			{
				HeaderTitle.NavigateUrl = HomeUrl;
				HeaderTitle.Text = Title;
				ControlHelper.SetTitleIfNone(HeaderTitle, "The Title Of This Blog.");
			}
			
			if (null != this.FindControl("HeaderSubTitle"))
			{
				HeaderSubTitle.Text = Subtitle;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.DataBind();
		}
	}
}

