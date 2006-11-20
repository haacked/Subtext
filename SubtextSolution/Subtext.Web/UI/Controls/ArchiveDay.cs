using System;
using System.Globalization;
using Subtext.Framework.Data;
using Subtext.Framework;
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
	///		Summary description for ArchiveDay.
	/// </summary>
	public  class ArchiveDay : BaseControl
	{
		protected Subtext.Web.UI.Controls.Day SingleDay;
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{
				try
				{
					DateTime dt = WebPathStripper.GetDateFromRequest(Request.Path,"archive");
					SingleDay.CurrentDay = Cacher.GetDay(dt, CacheDuration.Short);
					Subtext.Web.UI.Globals.SetTitle(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1} Entries",CurrentBlog.Title,dt.ToString("D", CultureInfo.CurrentCulture)),Context);
				}
				catch(System.FormatException)
				{
					//Somebody probably is messing with the url.
                    //404 is set in filenotfound - DF
					Response.Redirect("~/SystemMessages/FileNotFound.aspx");
				}
			}
		}
	}
}


