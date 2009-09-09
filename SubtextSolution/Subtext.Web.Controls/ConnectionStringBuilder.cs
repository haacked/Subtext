#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Scripting;

namespace Subtext.Web.Controls
{ //TODO: Review.
    /// <summary>
    /// Control used to specify a connection string.
    /// </summary>
    [ValidationProperty("ConnectionString")]
    public class ConnectionStringBuilder : WebControl, INamingContainer
    {
        const string AuthModeControlId = "radioConnectionStringBuilderAuthMode";
        const string CheckboxControlId = "chkUseConnectionStringInWebConfig";
        const string ConnectionResultControlId = "lblConnectionStringBuilderConnectionResult";
        const string ConnectionStringControlId = "txtConnectionStringBuilderConnectionString";
        const string DatabaseNameControlId = "lstConnectionStringBuilderDatabaseName";
        const string DescriptionControlId = "ltlConnectionStringBuilderDescriptionText";

        const string MachineNameControlId = "lstConnectionStringBuilderMachineName";
        const string OtherMachineNameControlId = "txtConnectionStringBuilderOtherMachineName";
        const string PasswordControlId = "txtConnectionStringBuilderPassword";
        const string RefreshDatabaseControlId = "lnkBtnConnectionStringBuilderRefreshDatabase";
        const string TestConnectionControlId = "btnConnectionStringBuilderTestConnection";
        const string TitleControlId = "ltlConnectionStringBuilderTitleText";
        const string UsernameControlId = "txtConnectionStringBuilderUsername";
        private ConnectionString _connStr = Scripting.ConnectionString.Empty;
        protected RadioButtonList authMode = new RadioButtonList();
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "conn")] protected Label connResult = new Label();
        protected DropDownList databaseName = new DropDownList();
        protected DropDownList machineName = new DropDownList();
        protected TextBox otherMachineName = new TextBox();
        protected TextBox password = new TextBox();
        protected LinkButton refreshDatabase = new LinkButton();
        protected Button testConnection = new Button();
        protected TextBox username = new TextBox();

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
                {
                    return (bool)ViewState["AllowWebConfigOverride"];
                }
                return false;
            }
            set { ViewState["AllowWebConfigOverride"] = value; }
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
                {
                    return ConfigurationManager.AppSettings["ConnectionString"];
                }
                return ConnectionStringTextBox.Text;
            }
            set
            {
                _connStr = Scripting.ConnectionString.Parse(value);
                if(ConnectionStringTextBox != null)
                {
                    ConnectionStringTextBox.Text = value;
                }
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
                {
                    return ViewState["Title"] as string;
                }
                return string.Empty;
            }
            set { ViewState["Title"] = value; }
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
                {
                    return ViewState["Description"] as string;
                }
                return string.Empty;
            }
            set { ViewState["Description"] = value; }
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

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            authMode.SelectedIndexChanged += authMode_SelectedIndexChanged;
            testConnection.Click += testConnection_Click;
            refreshDatabase.Click += refreshDatabase_Click;
        }

        /// <summary>
        /// Called during the page init event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            TitleLiteralControl.Text = Title;
            DescriptionLiteralControl.Text = Description;
            base.OnPreRender(e);
        }

        /// <summary>
        /// Creates the child controls for this control.
        /// </summary>
        protected override void CreateChildControls()
        {
            var table = new HtmlTable();
            table.CellPadding = 3;
            table.CellSpacing = 0;

            // Create header row
            var headerRow = new HtmlTableRow();
            headerRow.BgColor = "#EEEEEE";
            var fieldCell = new HtmlTableCell("TH");
            fieldCell.ColSpan = 2;
            var strongControl = new HtmlGenericControl("strong");
            var titleControl = new LiteralControl("Connection String");
            titleControl.ID = TitleControlId;

            strongControl.Controls.Add(titleControl);
            fieldCell.Controls.Add(strongControl);
            headerRow.Cells.Add(fieldCell);
            table.Rows.Add(headerRow);

            // Create Text Input Row
            var row = new HtmlTableRow();
            row.VAlign = "top";
            var questionCell = new HtmlTableCell();


            //Build low-level Textbox
            var textbox = new TextBox();
            textbox.ID = ConnectionStringControlId;
            questionCell.Controls.Add(textbox);
            row.Cells.Add(questionCell);

            //Checkbox to use connection string in web.config
            if(AllowWebConfigOverride)
            {
                var checkbox = new CheckBox();
                checkbox.ID = CheckboxControlId;
                checkbox.Text = "Use Connection String In Web.config";
                //This next line is a hack since we don't yet know the client id yet.
                //checkbox.Attributes["onclick"] = "if(this.checked) {" + this.ID + "_" + textbox.ClientID + ".disabled = true;} else {" + this.ID + "_" + textbox.ClientID + ".disabled = false;} ;";
                questionCell.Controls.Add(new LiteralControl("<br />"));
                questionCell.Controls.Add(checkbox);
            }

            var descriptionCell = new HtmlTableCell();
            var descriptionText = new LiteralControl(Description);
            descriptionText.ID = DescriptionControlId;
            descriptionCell.Controls.Add(descriptionText);
            row.Cells.Add(descriptionCell);

            table.Rows.Add(row);
            Controls.Add(table);

            base.CreateChildControls();
        }

        /// <summary>
        /// Build the advanced Connection String Builder
        /// Build the UI, attach events, and populate the fields
        /// </summary>
        private HtmlTable BuildAdvancedBuilder()
        {
            HtmlTable connBuilderTable = BuildMainTable();
            return connBuilderTable;
        }

        private void SetConnectionString()
        {
            _connStr.TrustedConnection = authMode.SelectedValue.Equals("win");
            if(String.IsNullOrEmpty(otherMachineName.Text.Trim()))
            {
                _connStr.Server = machineName.SelectedValue;
            }
            else
            {
                _connStr.Server = otherMachineName.Text;
            }

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

        private void authMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(authMode.SelectedValue.Equals("win"))
            {
                username.Enabled = false;
                password.Enabled = false;
                SetConnectionString();
            }
            else if(authMode.SelectedValue.Equals("sql"))
            {
                username.Enabled = true;
                password.Enabled = true;
                username.Text = _connStr.UserId;
                password.Text = _connStr.Password;
            }
        }

        private void refreshDatabase_Click(object sender, EventArgs e)
        {
            SetConnectionString();
            connResult.Text = "";
        }

        private void testConnection_Click(object sender, EventArgs e)
        {
            try
            {
                SetConnectionString();
                using(var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if(conn.State == ConnectionState.Open)
                    {
                        connResult.Text = "Connection Succeded";
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                connResult.Text = "Connection Failed:" + ex.Message;
            }
        }

        protected void LoadData()
        {
            Page.Trace.Write("onLoad");
            connResult.Text = "";
            if(!Page.IsPostBack)
            {
                if(_connStr != null)
                {
                    if(machineName.Items.FindByValue(_connStr.Server) != null)
                    {
                        machineName.SelectedValue = _connStr.Server;
                    }
                    else
                    {
                        otherMachineName.Text = _connStr.Server;
                    }

                    if(_connStr.TrustedConnection)
                    {
                        authMode.SelectedValue = "win";
                        username.Enabled = false;
                        password.Enabled = false;
                    }
                    else
                    {
                        authMode.SelectedValue = "sql";
                        username.Enabled = true;
                        password.Enabled = true;
                        username.Text = _connStr.UserId;
                        password.Text = _connStr.Password;
                    }
                }
            }
        }

        #region UI Builder

        /// <summary>
        /// Build the main UI Table
        /// </summary>
        private HtmlTable BuildMainTable()
        {
            var mainTable = new HtmlTable();
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
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.InnerHtml = "Server Name";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(machineName);
            cell.Controls.Add(new LiteralControl("<br />"));
            cell.Controls.Add(otherMachineName);
            machineName.ID = MachineNameControlId;
            machineName.EnableViewState = true;
            otherMachineName.ID = OtherMachineNameControlId;
            otherMachineName.TextMode = TextBoxMode.SingleLine;
            Page.Trace.Write("machineName");
            row.Cells.Add(cell);

            return row;
        }

        private HtmlTableRow BuildAuthenticationRow()
        {
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.InnerHtml = "Authentication";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            authMode.ID = AuthModeControlId;
            authMode.AutoPostBack = true;
            authMode.Items.Add(new ListItem("SQL", "sql"));
            authMode.Items.Add(new ListItem("Trusted", "win"));
            cell.Controls.Add(authMode);
            row.Cells.Add(cell);

            return row;
        }

        private HtmlTableRow BuildUsernameRow()
        {
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.InnerHtml = "Username";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            username.ID = UsernameControlId;
            username.TextMode = TextBoxMode.SingleLine;
            cell.Controls.Add(username);
            row.Cells.Add(cell);

            return row;
        }

        private HtmlTableRow BuildPasswordRow()
        {
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.InnerHtml = "Password";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            password.ID = PasswordControlId;
            password.TextMode = TextBoxMode.SingleLine;
            cell.Controls.Add(password);
            row.Cells.Add(cell);

            return row;
        }

        private HtmlTableRow BuildDatabaseRow()
        {
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.InnerHtml = "Database";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            databaseName.ID = DatabaseNameControlId;
            refreshDatabase.ID = RefreshDatabaseControlId;
            refreshDatabase.Text = "Refresh Databases";
            cell.Controls.Add(databaseName);
            cell.Controls.Add(new LiteralControl("<br />"));
            cell.Controls.Add(refreshDatabase);
            row.Cells.Add(cell);

            return row;
        }

        private HtmlTableRow BuildTestConnRow()
        {
            var row = new HtmlTableRow();
            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.ColSpan = 2;
            testConnection.ID = TestConnectionControlId;
            testConnection.Text = "Test Connection";
            connResult.ID = ConnectionResultControlId;
            connResult.Text = "";
            cell.Controls.Add(testConnection);
            cell.Controls.Add(new LiteralControl("<br />"));
            cell.Controls.Add(connResult);
            row.Cells.Add(cell);

            return row;
        }

        #endregion // UI Builder
    }
}