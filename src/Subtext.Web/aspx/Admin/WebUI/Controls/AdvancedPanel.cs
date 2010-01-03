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

using System.Web.UI;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.WebUI
{
    // TODO: Designer, design-time enhancements
    // TODO: Collapsible property, don't add link to ctls if false -- interaction with Collapsed tests?
    // TODO: Properties should not have dependencies on each other; Setting one property should not 
    // affect other properties

    [ToolboxData("<{0}:AdvancedPanel runat=\"server\"></{0}:AdvancedPanel>")]
    public class AdvancedPanel : CollapsiblePanel
    {
        /// <summary>
        /// Creates a new <see cref="AdvancedPanel"/> instance and 
        /// sets some initial properties specific to the admin tool.
        /// </summary>
        public AdvancedPanel()
        {
            LinkImage = "~/aspx/admin/images/toggle_gray_up.gif";
            LinkImageCollapsed = "~/aspx/admin/images/toggle_gray_down.gif";
            CssClass = "section";
        }
    }
}