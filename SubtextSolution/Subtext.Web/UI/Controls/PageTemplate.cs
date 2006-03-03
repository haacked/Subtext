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
