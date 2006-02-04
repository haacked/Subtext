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
using System.Globalization;
using System.Web.UI;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for CachedColumnControl.
	/// </summary>
	[PartialCaching(30,null,null,"Blogger",true)]
	public class CachedColumnControl : BaseControl
	{
		public CachedColumnControl()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render (writer);
			#if DEBUG
			   
				writer.Write("<font size = \"1\">Cached @ " + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "</font>");
				writer.Write("<font size = \"1\">Control " + this.GetType().ToString() + "</font>");
			
			#endif
		}
	}
}
