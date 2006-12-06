using System;

namespace Subtext.Web.Skins._System
{
	public partial class Login : System.Web.UI.MasterPage
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
		///</summary>
		///
		///<param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
		protected override void OnPreRender(EventArgs e)
		{
			loginControl.DestinationPageUrl = DestinationUrl;
			base.OnPreRender(e);
		}

		public string DestinationUrl
		{
			get { return this.destinationUrl ?? "~/Admin/"; }
			set { this.destinationUrl = value; }
		}

		string destinationUrl;
	}
}
