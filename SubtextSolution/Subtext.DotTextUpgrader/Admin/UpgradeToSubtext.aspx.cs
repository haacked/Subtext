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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Subtext.Scripting;

namespace Subtext.DotTextUpgrader.Admin
{
	/// <summary>
	/// Starts the upgrade process for upgrading DotText to Subtext.
	/// </summary>
	public class UpgradeToSubtext : System.Web.UI.Page
	{
		#region Control Declarations
		protected System.Web.UI.WebControls.Panel pnlFoundConnectionString;
		protected System.Web.UI.WebControls.Panel pnlConnectionStringNotFound;
		protected System.Web.UI.WebControls.TextBox txtConnectionString;
		protected System.Web.UI.WebControls.Button btnNext;
		protected System.Web.UI.WebControls.Panel pnlStep1;
		protected System.Web.UI.WebControls.Panel pnlStep2;
		protected System.Web.UI.WebControls.Button btnConfirmString;
		protected System.Web.UI.WebControls.Label lblConnectionString;
		protected System.Web.UI.WebControls.Label lblDatabaseName;
		#endregion
	
		protected override void OnLoad(EventArgs e)
		{
			string connectionString = ReadConnectionStringFromOldWebConfig();
			if(connectionString != null)
			{
				pnlFoundConnectionString.Visible = true;
				this.lblConnectionString.Text = connectionString;
				ConnectionString connectionInfo = ConnectionString.Parse(connectionString);
				this.lblDatabaseName.Text = connectionInfo.Database;
			}
			else
				this.pnlConnectionStringNotFound.Visible = true;

			base.OnLoad (e);
		}

		string ReadConnectionStringFromOldWebConfig()
		{
			string configPath = Path.Combine(Request.PhysicalApplicationPath, "Web.config");
			string contents = ReadFile(configPath);
			Regex regex = new Regex(@"<DbProvider\s+type\s*=\s*""Dottext.Framework.Data.SqlDataProvider,\s*Dottext.Framework""\s*connectionString\s*=\s*""(?<conn>[^""]+)""\s*/>", RegexOptions.IgnoreCase);
			Match match = regex.Match(contents);
			if(match.Success)
				return  match.Groups["conn"].Value;
			else
				return null;
		}

		private string ReadFile(string path)
		{
			using(StreamReader reader = new StreamReader(path, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
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
			this.btnConfirmString.Click += new System.EventHandler(this.btnConfirmString_Click);
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);

		}
		#endregion

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			pnlStep1.Visible = false;
			pnlStep2.Visible = true;
			DotText095Upgrader upgrader = new DotText095Upgrader();
			upgrader.Upgrade(this.lblConnectionString.Text);
		}

		private void btnConfirmString_Click(object sender, System.EventArgs e)
		{
			pnlFoundConnectionString.Visible = true;
			this.lblConnectionString.Text = this.txtConnectionString.Text;
		}
	}
}
