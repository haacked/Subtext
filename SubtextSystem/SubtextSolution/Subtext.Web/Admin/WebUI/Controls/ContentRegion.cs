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

// Aped from: Paul Wilson @ www.wilsondotnet.com

using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
	[ToolboxData("<{0}:ContentRegion runat=server></{0}:ContentRegion>")]
	public class PlaceHolder : System.Web.UI.WebControls.Panel
	{
		public PlaceHolder() {
			base.BackColor = Color.WhiteSmoke;
			base.Width = new Unit("100%", CultureInfo.InvariantCulture);
		}

		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer) {}
		public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer) {}
	}
}

