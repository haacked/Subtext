using System;
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
			   
				writer.Write("<font size = \"1\">Cached @ " + DateTime.Now.ToString() + "</font>");
				writer.Write("<font size = \"1\">Control " + this.GetType().ToString() + "</font>");
			
			#endif
		}
	}
}
