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
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.WebControls
{
	/// <summary>
	/// Use this to link to the rss feed and do not specify the NavigateUrl property. 
	/// It'll fill this in for you.
	/// </summary>
	public class RssHyperLink : HyperLink
	{
		/// <summary>
		/// Overrides the NavigateUrl property to point too the RSS feed 
		/// and raises the <see cref="E:System.Web.UI.Control.PreRender"/>
		/// event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			if(Config.CurrentBlog.FeedBurnerEnabled)
			{
				NavigateUrl = Config.CurrentBlog.UrlFormats.FeedBurnerUrl.ToString();
			}
			else 
			{
				NavigateUrl = Config.CurrentBlog.UrlFormats.RssUrl.ToString();
			}
			base.OnPreRender (e);
		}
	}
}
