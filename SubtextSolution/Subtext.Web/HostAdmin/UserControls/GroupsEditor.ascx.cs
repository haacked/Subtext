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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using System.Data.SqlClient;
using Subtext.Framework.Data;

namespace Subtext.Web.HostAdmin.UserControls
{
	/// <summary>
	///	User control used to create, edit and delete Blog Groups.
	/// </summary>
	public partial class GroupsEditor : UserControl
	{
        const string VSKEY_GROUPID = "VS_GROUPID";

		#region Declared Controls
		protected Button btnAddNewGroup = new Button();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            this.btnAddNewGroup.Click += btnAddNewGroup_Click;
			
			btnAddNewGroup.CssClass = "button";
			btnAddNewGroup.Text = "New Blog Group";
			((HostAdminTemplate)this.Page.Master).AddSidebarControl(btnAddNewGroup);
            
            if (!IsPostBack)
			{
				this.chkShowInactive.Checked = false;
			}
			BindList();
		}

		private void BindList()
		{
			this.pnlResults.Visible = true;
			this.pnlEdit.Visible = false;

			IList<BlogGroup> groups = Config.ListBlogGroups(!chkShowInactive.Checked);

            if (groups.Count > 0)
			{
                this.rprGroupsList.Visible = true;
                this.rprGroupsList.DataSource = groups;
                this.rprGroupsList.DataBind();
				this.lblNoMessages.Visible = false;
			}
			else
			{
                this.rprGroupsList.Visible = false;
				this.lblNoMessages.Visible = true;	
			}

		}

		void BindEdit()
		{
			this.pnlResults.Visible = false;
			this.pnlEdit.Visible = true;
			
			BindEditHelp();

            if(!CreatingGroup)
			{
                SqlDataReader reader = (SqlDataReader) DbProvider.Instance().GetBlogGroup(GroupId, false);
                if (reader.Read())
                {
                    this.txtTitle.Text = reader.GetString(reader.GetOrdinal("Title"));
                    this.txtDescription.Text = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description"));
                    this.txtDisplayOrder.Text = reader.IsDBNull(reader.GetOrdinal("DisplayOrder")) ? string.Empty : reader.GetInt32(reader.GetOrdinal("DisplayOrder")).ToString();
                    hfActive.Value = Convert.ToString(reader.GetBoolean(reader.GetOrdinal("Active")));
                }
			}					
		}

		// Contains the various help strings
		void BindEditHelp()
		{
			#region Help Tool Tip Text
			this.blogEditorHelp.HelpText = "<p>Use this page to manage the blogs groups for this server. " 
				+ "For more information on configuring blogs, see the <a href=\'http://www.subtextproject.com/Home/Docs/Configuration/tabid/112/Default.aspx\' target=\'_blank\'>configuration docs</a> (opens a new window)."
				+ "</p>";
			
			#endregion
		}

		/// <summary>
		/// Gets or sets the group id.
		/// </summary>
		/// <value></value>
		public int GroupId
		{
			get
			{
				if (ViewState[VSKEY_GROUPID] != null)
					return (int)ViewState[VSKEY_GROUPID];
				else
					return NullValue.NullInt32;
			}
			set { ViewState[VSKEY_GROUPID] = value; }
		}

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        bool CreatingGroup
		{
			get
			{
                return GroupId == NullValue.NullInt32;
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

		}
		#endregion

		protected void chkShowInactive_CheckedChanged(object sender, EventArgs e)
		{
			BindList();
		}

		private void btnAddNewGroup_Click(object sender, EventArgs e)
		{
            this.GroupId = NullValue.NullInt32;
			this.txtTitle.Text = string.Empty;
			this.txtDescription.Text = string.Empty;
			this.txtDisplayOrder.Text = string.Empty;
			BindEdit();
		}

		protected void rprGroupsList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			switch (e.CommandName.ToLower(CultureInfo.InvariantCulture)) 
			{
				case "edit":
                    string[] arr = ((string)e.CommandArgument).Split('|');
                    GroupId = Convert.ToInt32(arr[0]);
                    IsActive = Convert.ToBoolean(arr[1]);
					BindEdit();
					break;

                case "toggleactive":
                    string[] arr1 = ((string)e.CommandArgument).Split('|');
                    GroupId = Convert.ToInt32(arr1[0]);
                    IsActive = Convert.ToBoolean(arr1[1]);
                    ToggleActive();
                    break;

                case "delete":
                    string[] arr2 = ((string)e.CommandArgument).Split('|');
                    GroupId = Convert.ToInt32(arr2[0]);
                    IsActive = Convert.ToBoolean(arr2[1]);
                    DeleteGroup();
                    break;
				
				default:
					break;
			}
		}

        private void DeleteGroup(){
            if (DbProvider.Instance().DeleteBlogGroup(GroupId))
                BindList();
        }

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveConfig();
		}

		private void SaveConfig()
		{
            try
            {
                if (GroupId != NullValue.NullInt32)
                {
                    SaveGroupEdits();
                }
                else
                {
                    SaveNewGroup();
                }
                BindList();
                return;
            }
            catch (BaseBlogConfigurationException e)
            {
                this.messagePanel.ShowError(e.Message);
            }
            BindEdit();
		}

		// Saves a new blog group.  Any exceptions are propagated up to the caller.
        void SaveNewGroup()
		{
            int d;
            if (!Int32.TryParse(this.txtDisplayOrder.Text, out d))
                d = NullValue.NullInt32;
            if (DbProvider.Instance().InsertBlogGroup(this.txtTitle.Text, true, d, this.txtDescription.Text) > 0)
			{
				this.messagePanel.ShowMessage("Blog Group Created.");
			}
			else
			{
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
			}		
		}

		// Saves changes to a blog group.  Any exceptions are propagated up to the caller.
        void SaveGroupEdits()
        {
            int d;
            if (!Int32.TryParse(this.txtDisplayOrder.Text, out d))
                d = NullValue.NullInt32;
            if (DbProvider.Instance().UpdateBlogGroup(GroupId, this.txtTitle.Text, Convert.ToBoolean(hfActive.Value), d, this.txtDescription.Text))
			{
				this.messagePanel.ShowMessage("Blog Group Saved.");
			}
			else
			{
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
			}
		}
        
		protected static string ToggleActiveString(bool active)
		{
			if(active)
				return "Deactivate";
			else
				return "Activate";
		}

		void ToggleActive()
		{
            try
			{
                BlogGroup group = Config.GetBlogGroup(GroupId, false);
                DbProvider.Instance().UpdateBlogGroup(GroupId, group.Title, !IsActive, (int) DataHelper.CheckNull(group.DisplayOrder), group.Description); 
			}
			catch(BaseBlogConfigurationException e)
			{
				this.messagePanel.ShowError(e.Message);
			}

			BindList();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.messagePanel.ShowMessage("Blog Group Update Cancelled. Nothing to see here.");
			BindList();
		}

		
			
		
	}
}
