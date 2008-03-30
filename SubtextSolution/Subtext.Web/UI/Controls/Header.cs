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

		public Header(){}

		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		
			if (null != this.FindControl("HeaderTitle"))
			{
				HeaderTitle.NavigateUrl = CurrentBlog.HomeVirtualUrl;
				HeaderTitle.Text = CurrentBlog.Title;
				ControlHelper.SetTitleIfNone(HeaderTitle, "The Title Of This Blog.");
			}
			
			if (null != this.FindControl("HeaderSubTitle"))
			{
				HeaderSubTitle.Text = CurrentBlog.SubTitle;
			}
		}
	}
}

