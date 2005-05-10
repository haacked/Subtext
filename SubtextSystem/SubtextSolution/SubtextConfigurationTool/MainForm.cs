using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SubtextConfigurationTool
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		SubtextConfigFileEditor editor = null;
		public ArrayList webApps = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbWebApps;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TextBox txtDatabase;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.RadioButton radTrusted;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton radSqlAuth;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnTestConnection;
		private System.Windows.Forms.Label label8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cmbWebApps = new System.Windows.Forms.ComboBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.radSqlAuth = new System.Windows.Forms.RadioButton();
			this.radTrusted = new System.Windows.Forms.RadioButton();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtDatabase = new System.Windows.Forms.TextBox();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnTestConnection = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "IIS Apps";
			// 
			// cmbWebApps
			// 
			this.cmbWebApps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbWebApps.DisplayMember = "FriendlyName";
			this.cmbWebApps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbWebApps.Location = new System.Drawing.Point(64, 48);
			this.cmbWebApps.Name = "cmbWebApps";
			this.cmbWebApps.Size = new System.Drawing.Size(304, 21);
			this.cmbWebApps.TabIndex = 0;
			this.cmbWebApps.ValueMember = "PhysicalPath";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnRefresh.Location = new System.Drawing.Point(376, 48);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh Apps";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(448, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Please select the Web Application in which you installed Subtext and click \"Load " +
				"Config\".";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnSelect);
			this.groupBox1.Controls.Add(this.btnRefresh);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmbWebApps);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(464, 112);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "IIS Application";
			// 
			// btnSelect
			// 
			this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSelect.Location = new System.Drawing.Point(376, 80);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 2;
			this.btnSelect.Text = "Load Config";
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.radSqlAuth);
			this.groupBox2.Controls.Add(this.radTrusted);
			this.groupBox2.Controls.Add(this.txtPassword);
			this.groupBox2.Controls.Add(this.txtDatabase);
			this.groupBox2.Controls.Add(this.txtServer);
			this.groupBox2.Controls.Add(this.txtUser);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(8, 200);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(464, 152);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Connection String";
			// 
			// label7
			// 
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(8, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 23);
			this.label7.TabIndex = 10;
			this.label7.Text = "Authentication";
			// 
			// radSqlAuth
			// 
			this.radSqlAuth.Checked = true;
			this.radSqlAuth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radSqlAuth.Location = new System.Drawing.Point(200, 16);
			this.radSqlAuth.Name = "radSqlAuth";
			this.radSqlAuth.TabIndex = 4;
			this.radSqlAuth.TabStop = true;
			this.radSqlAuth.Text = "SQL";
			this.radSqlAuth.CheckedChanged += new System.EventHandler(this.radSqlAuth_CheckedChanged);
			// 
			// radTrusted
			// 
			this.radTrusted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radTrusted.Location = new System.Drawing.Point(96, 16);
			this.radTrusted.Name = "radTrusted";
			this.radTrusted.TabIndex = 3;
			this.radTrusted.Text = "Trusted";
			this.radTrusted.CheckedChanged += new System.EventHandler(this.radTrusted_CheckedChanged);
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(80, 120);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(376, 20);
			this.txtPassword.TabIndex = 8;
			this.txtPassword.Text = "";
			// 
			// txtDatabase
			// 
			this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDatabase.Location = new System.Drawing.Point(80, 72);
			this.txtDatabase.Name = "txtDatabase";
			this.txtDatabase.Size = new System.Drawing.Size(376, 20);
			this.txtDatabase.TabIndex = 6;
			this.txtDatabase.Text = "";
			// 
			// txtServer
			// 
			this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtServer.Location = new System.Drawing.Point(80, 48);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(376, 20);
			this.txtServer.TabIndex = 5;
			this.txtServer.Text = "";
			// 
			// txtUser
			// 
			this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUser.Location = new System.Drawing.Point(80, 96);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(376, 20);
			this.txtUser.TabIndex = 7;
			this.txtUser.Text = "";
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(8, 72);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 23);
			this.label6.TabIndex = 3;
			this.label6.Text = "Database:";
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(8, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 23);
			this.label5.TabIndex = 2;
			this.label5.Text = "Server:";
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(8, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 1;
			this.label4.Text = "Password:";
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(8, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "User:";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSave.Location = new System.Drawing.Point(384, 360);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(88, 23);
			this.btnSave.TabIndex = 10;
			this.btnSave.Text = "Save Changes";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnTestConnection
			// 
			this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnTestConnection.Location = new System.Drawing.Point(8, 360);
			this.btnTestConnection.Name = "btnTestConnection";
			this.btnTestConnection.Size = new System.Drawing.Size(104, 23);
			this.btnTestConnection.TabIndex = 9;
			this.btnTestConnection.Text = "Test Connection";
			this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label8.Location = new System.Drawing.Point(8, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(464, 56);
			this.label8.TabIndex = 11;
			this.label8.Text = @"The purpose of this simple utility is to make it dreadfully easy to edit your Subtext Web.config file with the appropriate connection string so that it can connect to your Subtext database.  After you change this setting, open a browser to your blog and finish the configuration.  You must have write permissions to web.config to use this tool.";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 398);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnTestConnection);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Name = "MainForm";
			this.Text = "Subtext Configuration";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new MainForm());
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			this.webApps = IISHelper.EnumerateVirtualDirectories();
			this.cmbWebApps.DataSource = this.webApps;
		}

		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			string path = (string)this.cmbWebApps.SelectedValue;
			string configPath = Path.Combine(path, "Web.config");
			try
			{
				editor = new SubtextConfigFileEditor(configPath);
				this.radTrusted.Checked = editor.ConnectionString.TrustedConnection;
				this.txtServer.Text = editor.ConnectionString.Server;
				this.txtDatabase.Text = editor.ConnectionString.Database;

				if(!editor.ConnectionString.TrustedConnection)
				{
					this.txtUser.Text = editor.ConnectionString.UserID;
					this.txtPassword.Text = editor.ConnectionString.Password;	
				}
			}
			catch(Exception exc)
			{
				MessageBox.Show("An error occurred. " + Environment.NewLine + exc.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SetConnectionString();
			
			this.editor.Save();
			MessageBox.Show("Changes Saved!");
		}

		void SetConnectionString()
		{
			this.editor.ConnectionString.TrustedConnection = this.radTrusted.Checked;
			this.editor.ConnectionString.Server = this.txtServer.Text;
			this.editor.ConnectionString.Database = this.txtDatabase.Text;
			
			if(!this.editor.ConnectionString.TrustedConnection)
			{
				this.editor.ConnectionString.UserID = this.txtUser.Text;
				this.editor.ConnectionString.Password = this.txtPassword.Text;
			}
			else
			{
				this.editor.ConnectionString.UserID = string.Empty;
				this.editor.ConnectionString.Password = string.Empty;
			}
		}

		private void radSqlAuth_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtUser.Enabled = true;
			this.txtPassword.Enabled = true;
		}

		private void radTrusted_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtUser.Enabled = false;
			this.txtPassword.Enabled = false;
		}

		private void btnTestConnection_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetConnectionString();
				string connectionStr = this.editor.ConnectionString.ConnectionString;
				using(SqlConnection conn = new SqlConnection(connectionStr))
				{
					conn.Open();
					if(conn.State == ConnectionState.Open)
					{
						MessageBox.Show("Connection Succeeded");
						return;
					}
				}
			}
			catch(Exception)
			{
				
			}
			MessageBox.Show("Connection Failed");
		}
	}
}
