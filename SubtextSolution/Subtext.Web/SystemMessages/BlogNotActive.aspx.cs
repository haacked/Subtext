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
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Routing;

namespace Subtext.Web
{
	/// <summary>
	/// Displays the blog not active message.
	/// </summary>
	public partial class BlogNotActive : RoutablePage
	{
		protected override void OnLoad(EventArgs e)
		{
            base.OnLoad(e);
			try
			{
				if(!Config.CurrentBlog.IsActive)
				{
					plcInactiveBlogMessage.Visible = true;
					plcNothingToSeeHere.Visible = false;
				}
				else
				{
                    lnkBlog.HRef = Url.BlogUrl();
				}
			}
			catch(BlogDoesNotExistException)
			{
				plcInactiveBlogMessage.Visible = true;
				plcNothingToSeeHere.Visible = false;
			}
		}
	}
}
