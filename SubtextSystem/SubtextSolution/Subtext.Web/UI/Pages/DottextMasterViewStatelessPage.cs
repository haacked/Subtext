using System;

namespace Subtext.Web.UI.Pages
{
	/// <summary>
	/// Summary description for DottextMasterViewStatelessPage.
	/// </summary>
	public class ViewStatelessPage : System.Web.UI.Page
	{
		public ViewStatelessPage()
		{
			this.EnableViewState = false;
		}

		protected override object LoadPageStateFromPersistenceMedium()
		{return null;}

		protected override void SavePageStateToPersistenceMedium(object viewState){}
	}
}
