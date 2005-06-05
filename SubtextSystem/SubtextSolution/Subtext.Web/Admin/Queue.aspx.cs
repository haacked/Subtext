using System;
using System.Globalization;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for Queue.
	/// </summary>
	public class Queue : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Literal Literal1;
		protected System.Web.UI.WebControls.Literal Literal2;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			Literal1.Text = Subtext.Framework.Util.ManagedThreadPool.ActiveThreads.ToString(CultureInfo.InvariantCulture);
			Literal2.Text = Subtext.Framework.Util.ManagedThreadPool.WaitingCallbacks.ToString(CultureInfo.InvariantCulture);
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
