using System;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Util;

using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
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
	public  class ArchiveCategory : BaseControl
	{
		protected Literal Title;
		protected DayCollection Days;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

		    int? categoryId = SubtextContext.RequestContext.GetIdFromRequest();
            if (categoryId != null)
            {
                Days.Days = Entries.GetPostsByCategoryID(Blog.ItemCount, categoryId.Value);
            }
		}
	}
}

