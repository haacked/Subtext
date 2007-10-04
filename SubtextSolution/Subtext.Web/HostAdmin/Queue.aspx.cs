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
using System.Globalization;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Shows some diagnostic information.
	/// </summary>
	public partial class Queue : HostAdminPage
	{	
		protected void Page_Load(object sender, EventArgs e)
		{
			this.ltlActiveThreads.Text = Subtext.Framework.Threading.ManagedThreadPool.ActiveThreads.ToString(CultureInfo.InvariantCulture);
            this.ltlWaitingCallbacks.Text = Subtext.Framework.Threading.ManagedThreadPool.WaitingCallbacks.ToString(CultureInfo.InvariantCulture);
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

		}
		#endregion
	}
}
