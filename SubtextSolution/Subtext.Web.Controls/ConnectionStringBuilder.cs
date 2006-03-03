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
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Scripting;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Control used to specify a connection string.
	/// </summary>
	[ValidationProperty("ConnectionString")]
	public class ConnectionStringBuilder : WebControl, INamingContainer
	{
		const string ConnectionStringControlId = "txtConnectionStringBuilderConnectionString";
		const string DescriptionControlId = "ltlConnectionStringBuilderDescriptionText";
		const string TitleControlId = "ltlConnectionStringBuilderTitleText";
		const string CheckboxControlId = "chkUseConnectionStringInWebConfig";

		const string MachineNameControlId = "lstConnectionStringBuilderMachineName";
		const string OtherMachineNameControlId = "txtConnectionStringBuilderOtherMachineName";
		const string AuthModeControlId = "radioConnectionStringBuilderAuthMode";
		const string UsernameControlId = "txtConnectionStringBuilderUsername";
		const string PasswordControlId = "txtConnectionStringBuilderPassword";
		const string DatabaseNameControlId = "lstConnectionStringBuilderDatabaseName";
		const string TestConnectionControlId = "btnConnectionStringBuilderTestConnection";
		const string RefreshDatabaseControlId = "lnkBtnConnectionStringBuilderRefreshDatabase";
		const string ConnectionResultControlId = "lblConnectionStringBuilderConnectionResult";

		protected System.Web.UI.WebControls.DropDownList machineName=new DropDownList();
		protected System.Web.UI.WebControls.TextBox otherMachineName=new TextBox();
		protected System.Web.UI.WebControls.RadioButtonList authMode=new RadioButtonList();
		protected System.Web.UI.WebControls.TextBox username=new TextBox();
		protected System.Web.UI.WebControls.DropDownList databaseName= new DropDownList();
		protected System.Web.UI.WebControls.Button testConnection=new Button();
		protected System.Web.UI.WebControls.TextBox password=new TextBox();
		protected System.Web.UI.WebControls.Label connResult=new Label();
		protected System.Web.UI.WebControls.LinkButton refreshDatabase=new LinkButton();


		private ConnectionString _connStr=Subtext.Scripting.ConnectionString.Empty;


		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			authMode.SelectedIndexChanged += new System.EventHandler(this.authMode_SelectedIndexChanged);
			testConnection.Click += new System.EventHandler(this.testConnection_Click);
			refreshDatabase.Click += new System.EventHandler(this.refreshDatabase_Click);
		}

		/// <summary>
		/// Called during the page init event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			this.TitleLiteralControl.Text = this.Title;
			this.DescriptionLiteralControl.Text = this.Description;
			base.OnPreRender(e);
		}

		/// <summary>
		/// Creates the child controls for this control.
		/// </summary>
		protected override void CreateChildControls()
		{
			HtmlTable table = new HtmlTable();
			table.CellPadding = 3;
			table.CellSpacing = 0;

			// Create header row
			HtmlTableRow headerRow = new HtmlTableRow();
			headerRow.BgColor = "#EEEEEE";
			HtmlTableCell fieldCell = new HtmlTableCell("TH");
			fieldCell.ColSpan = 2;
			HtmlGenericControl strongControl = new HtmlGenericControl("strong");
			LiteralControl titleControl = new LiteralControl("Connection String");
			titleControl.ID = TitleControlId;

			strongControl.Controls.Add(titleControl);
			fieldCell.Controls.Add(strongControl);
			headerRow.Cells.Add(fieldCell);
			table.Rows.Add(headerRow);

			// Create Text Input Row
			HtmlTableRow row = new HtmlTableRow();
			row.VAlign = "top";
			HtmlTableCell questionCell = new HtmlTableCell();

			if(CheckSQLDMO()) 
			{
				//Build advanced control
				HtmlTable connBuilderTable=BuildAdvancedBuilder();
				questionCell.Controls.Add(connBuilderTable);
			}
			else 
			{
				//Build low-level Textbox
				TextBox textbox = new TextBox();
				textbox.ID = ConnectionStringControlId;
				questionCell.Controls.Add(textbox);
			}
			row.Cells.Add(questionCell);

			//Checkbox to use connection string in web.config
			if(AllowWebConfigOverride)
			{
				CheckBox checkbox = new CheckBox();
				checkbox.ID = CheckboxControlId;
				checkbox.Text = "Use Connection String In Web.config";
				//This next line is a hack since we don't yet know the client id yet.
				//checkbox.Attributes["onclick"] = "if(this.checked) {" + this.ID + "_" + textbox.ClientID + ".disabled = true;} else {" + this.ID + "_" + textbox.ClientID + ".disabled = false;} ;";
				questionCell.Controls.Add(new LiteralControl("<br />"));
				questionCell.Controls.Add(checkbox);
			}

			HtmlTableCell descriptionCell = new HtmlTableCell();
			LiteralControl descriptionText = new LiteralControl(Description);
			descriptionText.ID = DescriptionControlId;
			descriptionCell.Controls.Add(descriptionText);
			row.Cells.Add(descriptionCell);
			
			table.Rows.Add(row);
			this.Controls.Add(table);

			if(CheckSQLDMO()) 
			{
				LoadData();
			}

			base.CreateChildControls();
		}

		private bool CheckSQLDMO() 
		{
			try 
			{
				SQLDMO.Application dmo = new SQLDMO.ApplicationClass();
				
			}
			catch(System.Runtime.InteropServices.COMException) 
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Build the advanced Connection String Builder
		/// Build the UI, attach events, and populate the fields
		/// </summary>
		private HtmlTable BuildAdvancedBuilder() 
		{
			HtmlTable connBuilderTable=BuildMainTable();
			return connBuilderTable;
		}


		#region UI Builder
		/// <summary>
		/// Build the main UI Table
		/// </summary>
		private HtmlTable BuildMainTable() 
		{
			HtmlTable mainTable = new HtmlTable();
			mainTable.Rows.Add(BuildServerNameRow());
			mainTable.Rows.Add(BuildAuthenticationRow());
			mainTable.Rows.Add(BuildUsernameRow());
			mainTable.Rows.Add(BuildPasswordRow());
			mainTable.Rows.Add(BuildDatabaseRow());
			mainTable.Rows.Add(BuildTestConnRow());
			return mainTable;
			
		}

		private HtmlTableRow BuildServerNameRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.InnerHtml="Server Name";
			row.Cells.Add(cell);

			cell=new HtmlTableCell();
			cell.Controls.Add(machineName);
			cell.Controls.Add(new LiteralControl("<br />"));
			cell.Controls.Add(otherMachineName);
			machineName.ID=MachineNameControlId;
			machineName.EnableViewState=true;
			otherMachineName.ID=OtherMachineNameControlId;
			otherMachineName.TextMode=TextBoxMode.SingleLine;
			Page.Trace.Write("machineName");
			row.Cells.Add(cell);

			return row;
		}

		private HtmlTableRow BuildAuthenticationRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.InnerHtml="Authentication";
			row.Cells.Add(cell);

			cell=new HtmlTableCell();
			authMode.ID=AuthModeControlId;
			authMode.AutoPostBack=true;
			authMode.Items.Add(new ListItem("SQL","sql"));
			authMode.Items.Add(new ListItem("Trusted","win"));
			cell.Controls.Add(authMode);
			row.Cells.Add(cell);

			return row;
		}

		private HtmlTableRow BuildUsernameRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.InnerHtml="Username";
			row.Cells.Add(cell);

			cell=new HtmlTableCell();
			username.ID=UsernameControlId;
			username.TextMode=TextBoxMode.SingleLine;
			cell.Controls.Add(username);
			row.Cells.Add(cell);

			return row;
		}

		private HtmlTableRow BuildPasswordRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.InnerHtml="Password";
			row.Cells.Add(cell);

			cell=new HtmlTableCell();
			password.ID=PasswordControlId;
			password.TextMode=TextBoxMode.SingleLine;
			cell.Controls.Add(password);
			row.Cells.Add(cell);

			return row;
		}

		private HtmlTableRow BuildDatabaseRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.InnerHtml="Database";
			row.Cells.Add(cell);

			cell=new HtmlTableCell();
			databaseName.ID=DatabaseNameControlId;
			refreshDatabase.ID=RefreshDatabaseControlId;
			refreshDatabase.Text="Refresh Databases";
			cell.Controls.Add(databaseName);
			cell.Controls.Add(new LiteralControl("<br />"));
			cell.Controls.Add(refreshDatabase);
			row.Cells.Add(cell);

			return row;
		}

		private HtmlTableRow BuildTestConnRow()
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell;

			cell=new HtmlTableCell();
			cell.ColSpan=2;
			testConnection.ID=TestConnectionControlId;
			testConnection.Text="Test Connection";
			connResult.ID=ConnectionResultControlId;
			connResult.Text="";
			cell.Controls.Add(testConnection);
			cell.Controls.Add(new LiteralControl("<br />"));
			cell.Controls.Add(connResult);
			row.Cells.Add(cell);

			return row;
		}

		#endregion // UI Builder


		/// <summary>
		/// Uses SQL-DMO to retrieve all servers available on the network
		/// </summary>
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



		/// <summary>
		/// Populate the databases dropdown list with the DBs found on the specified server
		/// </summary>
		private void PopulateDatabaseNamesCmb()
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
				databaseName.Items.Add(new ListItem("-- Error retrieving database list --","0"));
			}
			finally
			{
				sqlInstance.DisConnect();
			}
		}

		
		private void SetConnectionString()
		{
			_connStr.TrustedConnection = authMode.SelectedValue.Equals("win");
			if(otherMachineName.Text.Trim().Equals(""))
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
				PopulateDatabaseNamesCmb();
			}
			else if(authMode.SelectedValue.Equals("sql")) 
			{
				username.Enabled=true;
				password.Enabled=true;
				username.Text=_connStr.UserId;
				password.Text=_connStr.Password;
				if(!username.Text.Trim().Equals(String.Empty))
				{
					PopulateDatabaseNamesCmb();
				}
			}
		}

		private void refreshDatabase_Click(object sender, System.EventArgs e)
		{
			SetConnectionString();
			connResult.Text="";
			PopulateDatabaseNamesCmb();
		}

		private void testConnection_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetConnectionString();
				using(SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					if(conn.State == ConnectionState.Open)
					{
						connResult.Text="Connection Succeded";
						return;
					}
				}
			}
			catch(Exception ex)
			{
				connResult.Text="Connection Failed:" + ex.Message;
			}

			
		}

		/// <summary>
		/// Gets a value indicating whether [allow web config override].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [allow web config override]; otherwise, <c>false</c>.
		/// </value>
		[Browsable(true)]
		[DefaultValue("false")]
		[Category("Behavior")]
		[Description("Whether or not to display checkbox...")]
		public bool AllowWebConfigOverride
		{
			get
			{
				if(ViewState["AllowWebConfigOverride"] != null)
					return (bool)ViewState["AllowWebConfigOverride"];
				return false;
			}
			set
			{
				ViewState["AllowWebConfigOverride"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("")]
		[Category("Behavior")]
		[Description("The Connection String.")]
		public String ConnectionString
		{
			get
			{
				if(UseWebConfigCheckBox != null && UseWebConfigCheckBox.Checked)
					return System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
				if(CheckSQLDMO()) 
				{
					SetConnectionString();
					return _connStr.ToString();
				}
				else
					return ConnectionStringTextBox.Text;
			}
			set
			{
				_connStr = Subtext.Scripting.ConnectionString.Parse(value);
				if(ConnectionStringTextBox != null)
					ConnectionStringTextBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("Connection String")]
		[Category("Display")]
		[Description("Title of this control.")]
		public string Title
		{
			get
			{
				if(ViewState["Title"] != null)
					return ViewState["Title"] as string;
				return string.Empty;
			}
			set
			{
				ViewState["Title"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue("")]
		[Category("Display")]
		[Description("Description of what the connection string is used for.")]
		public string Description
		{
			get
			{
				if(ViewState["Description"] != null)
					return ViewState["Description"] as string;
				return string.Empty;
			}
			set
			{
				ViewState["Description"] = value;
			}
		}

		#region Accessors for Controls in page

		TextBox ConnectionStringTextBox
		{
			get
			{
				EnsureChildControls();
				return FindControl(ConnectionStringControlId) as TextBox;
			}
		}

		CheckBox UseWebConfigCheckBox
		{
			get
			{
				EnsureChildControls();
				return FindControl(CheckboxControlId) as CheckBox;
			}

		}

		LiteralControl DescriptionLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(DescriptionControlId) as LiteralControl;
			}
		}

		LiteralControl TitleLiteralControl
		{
			get
			{
				EnsureChildControls();
				return FindControl(TitleControlId) as LiteralControl;
			}
		}
		#endregion // Accessors for Controls in page

		protected void LoadData()
		{
			Page.Trace.Write("onLoad");
			connResult.Text="";
			if(!Page.IsPostBack) 
			{
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

					PopulateDatabaseNamesCmb();
				}
			}
		}
	}
}
