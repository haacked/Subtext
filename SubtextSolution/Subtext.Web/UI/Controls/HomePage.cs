using System;
using System.Web.UI;
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

	[PartialCaching(45, null, null, "Blogger", false)]
	public class HomePage : BaseControl
	{
		protected DayCollection HomePageDays;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			HomePageDays.Days = Entries.GetHomePageEntries(Blog.ItemCount);
		}
	}
}

