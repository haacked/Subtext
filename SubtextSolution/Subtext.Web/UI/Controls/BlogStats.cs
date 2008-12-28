using System;
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Configuration;

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
	using System;


	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public class BlogStats : CachedColumnControl
	{
		protected System.Web.UI.WebControls.Literal PostCount;
		protected System.Web.UI.WebControls.Literal StoryCount;
		protected System.Web.UI.WebControls.Literal CommentCount;
		protected System.Web.UI.WebControls.Literal PingTrackCount;


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{
				Blog info = Config.CurrentBlog;
				PostCount.Text = info.PostCount.ToString(CultureInfo.InvariantCulture);
				StoryCount.Text = info.StoryCount.ToString(CultureInfo.InvariantCulture);
				CommentCount.Text = info.CommentCount.ToString(CultureInfo.InvariantCulture);
				PingTrackCount.Text = info.PingTrackCount.ToString(CultureInfo.InvariantCulture);
			}
		}
	}
}

