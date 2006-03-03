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
using System.Web.UI.HtmlControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Renders a clickable help tooltip.  This class does not embed the 
	/// necessary scripts and CSS (against the general practice). Instead, 
	/// it relies on the user having declared helptooltip.js and helptooltip.css.
	/// </summary>
	public class HelpToolTip : HtmlContainerControl
	{
		/// <summary>
		/// <p>Renders this tool tip.  The format looks like: 
		/// &lt;a class="helplink" onclick="showHelpTip(event, 'help text'); 
		/// return false;" href="?"&gt;Label Text&lt;a&gt;
		/// </p>
		/// //TODO: Look into embedding helplink.js and helplink.css
		/// </summary>
		/// <param name="writer">Writer.</param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string format = @"<a class=""helpLink"" onclick=""showHelpTip(event, '{0}'); return false;"" href=""?"">";
			string helpText = HelpText.Replace("'", "\'");

			writer.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, format, helpText));
			this.RenderChildren(writer);
			writer.Write("</a>");
		}

		/// <summary>
		/// Gets or sets the Help Text.  This is the 
		/// text displayed when clicking on the tooltip.
		/// </summary>
		/// <value></value>
		public string HelpText
		{
			get
			{
				if(IsAttributeDefined("helptext"))
					return Attributes["helptext"];
				else
					return string.Empty;
			}
			set
			{
				Attributes["helptext"] = value;
			}
		}

		bool IsAttributeDefined(string name)
		{
			return ControlHelper.IsAttributeDefined(this, name);
		}
	}
}