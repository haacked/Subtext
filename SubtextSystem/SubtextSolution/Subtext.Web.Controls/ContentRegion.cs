using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off 
	/// of Paul Wilson's excellent demo found
	/// here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
	/// Very MINOR changes were made here. Thanks Paul.
	/// </summary>
	public class ContentRegion : Panel
	{
		/// <summary>
		/// Creates a new <see cref="ContentRegion"/> instance.
		/// </summary>
		public ContentRegion() : base()
		{
		}
		
		/// <summary>
		/// Initializes this Content Region.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnInit(EventArgs e)
		{
			base.BackColor = Color.WhiteSmoke;
			base.Width = new Unit("100%", CultureInfo.InvariantCulture);
			base.OnInit(e);
		}


		/// <summary>
		/// Renders the begin tag. In this case, a no-op.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
		}

		/// <summary>
		/// Renders the end tag.  In this case, a no-op.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
		}
 

 

	}
}
