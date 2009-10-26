#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Code Notes

/*
	The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off of Paul Wilson's excellect demo found
	here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
	
	Very MINOR changes were made here. Thanks Paul.
*/

#endregion

namespace Subtext.Web.UI.WebControls
{
    [ToolboxData("<{0}:ContentRegion runat=server></{0}:ContentRegion>")]
    public class ContentRegion : Panel
    {
        public ContentRegion()
        {
            base.BackColor = Color.WhiteSmoke;
            base.Width = new Unit("100%", CultureInfo.InvariantCulture);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }
    }
}