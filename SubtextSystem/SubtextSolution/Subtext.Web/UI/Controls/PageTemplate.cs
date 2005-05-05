using System;
using System.Web.UI;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for PageTemplate.
	/// </summary>
	public class PageTemplate : BaseControl
	{
		protected System.Web.UI.WebControls.PlaceHolder BodyControl;
		public PageTemplate()
		{

		}

		public virtual void AddBody(Control cntrl)
		{
			BodyControl.Controls.Add(cntrl);
		}
	}
}
