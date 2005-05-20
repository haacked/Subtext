#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
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
		public AdvancedPanel() : base()
		{
			this.LinkImage = "~/admin/resources/toggle_gray_up.gif";
			this.LinkImageCollapsed = "~/admin/resources/toggle_gray_down.gif";
		}
	}
}

