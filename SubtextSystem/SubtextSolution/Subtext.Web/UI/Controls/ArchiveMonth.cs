using System;
using System.Globalization;
using Subtext.Common.Data;
using Subtext.Framework.Util;

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
	///		Summary description for ArchiveMonth.
	/// </summary>
	public  class ArchiveMonth : Subtext.Web.UI.Controls.BaseControl
	{
		
		protected Subtext.Web.UI.Controls.EntryList Days;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			DateTime dt = WebPathStripper.GetDateFromRequest(Request.Path,"archive");
			Days.EntryListItems = Cacher.GetMonth(dt,CacheTime.Short,Context);
			Days.EntryListTitle = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} Entries", dt.ToString("MMMM yyyy", CultureInfo.CurrentCulture));
			Subtext.Web.UI.Globals.SetTitle(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1} Entries", CurrentBlog.Title, dt.ToString("MMMM yyyy", CultureInfo.CurrentCulture)),Context);
		}
	}
}

