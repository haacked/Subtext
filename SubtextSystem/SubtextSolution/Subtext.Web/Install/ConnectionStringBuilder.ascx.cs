namespace Subtext.Web.Install
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Scripting;
	using SQLDMO;

	/// <summary>
	///		Summary description for ConnectionStringBuilder.
	/// </summary>
	public class ConnectionStringBuilder : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList machineName;
		protected System.Web.UI.WebControls.TextBox otherMachineName;
		protected System.Web.UI.WebControls.RadioButtonList authMode;
		protected System.Web.UI.WebControls.TextBox username;
		protected System.Web.UI.WebControls.DropDownList databaseName;
		protected System.Web.UI.WebControls.Button testConnection;
		protected System.Web.UI.WebControls.TextBox password;
		protected System.Web.UI.WebControls.Label connResult;
		protected System.Web.UI.WebControls.LinkButton refreshDatabase;

		private ConnectionString _connStr;

		public String ConnStr 
		{
			get 
			{
				return _connStr.ToString();
			}

			set 
			{
				_connStr=ConnectionString.Parse(value);
			}
		}


		private void Page_Load(object sender, System.EventArgs e)
		{
			connResult.Text="";
			if(!Page.IsPostBack) {
				PopulateServerNameCmb();
				if(_connStr!=null) 
				{
					if(machineName.Items.FindByValue(_connStr.Server)!=null)
						machineName.SelectedValue=_connStr.Server;
					else
						otherMachineName.Text=_connStr.Server;

					if(_connStr.TrustedConnection)
					{
						authMode.SelectedValue="win";
						username.Enabled=false;
						password.Enabled=false;
					}
					else 
					{
						authMode.SelectedValue="sql";
						username.Enabled=true;
						password.Enabled=true;
						username.Text=_connStr.UserId;
						password.Text=_connStr.Password;
					}

					PopulateDatabaseNamesCmb(_connStr);
				}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.authMode.SelectedIndexChanged += new System.EventHandler(this.authMode_SelectedIndexChanged);
			this.testConnection.Click += new System.EventHandler(this.testConnection_Click);
			this.refreshDatabase.Click += new System.EventHandler(this.refreshDatabase_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void PopulateServerNameCmb() 
		{
			ArrayList serverNames = new ArrayList();
			SQLDMO.Application dmo = new SQLDMO.ApplicationClass();
			SQLDMO.NameList instances = dmo.ListAvailableSQLServers();
			foreach(string instance in instances) 
			{
				serverNames.Add(instance);
			}
			machineName.DataSource=serverNames;
			machineName.DataBind();
		}

		private void PopulateDatabaseNamesCmb(ConnectionString connStr)
		{

			ArrayList dbNames = new ArrayList();
			SQLDMO.SQLServer sqlInstance = new SQLDMO.SQLServerClass();
			try
			{
				if(_connStr.TrustedConnection) 
				{
					sqlInstance.LoginSecure=true;
					sqlInstance.Connect(_connStr.Server,null,null);
				}
				else
					sqlInstance.Connect(_connStr.Server,_connStr.UserId,_connStr.Password);
				foreach(SQLDMO.Database db in sqlInstance.Databases) 
				{
					dbNames.Add(db.Name);
				}
				databaseName.DataSource=dbNames;
				databaseName.DataBind();

				if(databaseName.Items.FindByValue(_connStr.Database)!=null) 
					databaseName.SelectedValue=_connStr.Database;
			}
			catch(Exception) 
			{
				connResult.Text="Error retrieving database list";
				databaseName.Items.Clear();
			}
			finally
			{
				sqlInstance.DisConnect();
			}
		}

		private void testConnection_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetConnectionString();
				using(SqlConnection conn = new SqlConnection(ConnStr))
				{
					conn.Open();
					if(conn.State == ConnectionState.Open)
					{
						connResult.Text="Connection Succeded";
						return;
					}
				}
			}
			catch(Exception)
			{
				
			}

			connResult.Text="Connection Failed";
		}

		private void SetConnectionString() 
		{
			_connStr.TrustedConnection = authMode.SelectedValue.Equals("win");
			if(machineName.SelectedIndex!=-1)
				_connStr.Server = machineName.SelectedValue;
			else
				_connStr.Server = otherMachineName.Text;
			_connStr.Database = databaseName.SelectedValue;
			
			if(!_connStr.TrustedConnection)
			{
				_connStr.UserId = username.Text;
				_connStr.Password = password.Text;
			}
			else
			{
				_connStr.UserId = string.Empty;
				_connStr.Password = string.Empty;
			}		
		}

		private void authMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(authMode.SelectedValue.Equals("win")) 
			{
				username.Enabled=false;
				password.Enabled=false;
				SetConnectionString();
				PopulateDatabaseNamesCmb(_connStr);
			}
			else if(authMode.SelectedValue.Equals("sql")) 
			{
				username.Enabled=true;
				password.Enabled=true;
				username.Text=_connStr.UserId;
				password.Text=_connStr.Password;
				if(!username.Text.Trim().Equals(String.Empty))
				{
					PopulateDatabaseNamesCmb(_connStr);
				}
			}
			
		}

		private void refreshDatabase_Click(object sender, System.EventArgs e)
		{
			SetConnectionString();
			PopulateDatabaseNamesCmb(_connStr);
		}
	}
}
