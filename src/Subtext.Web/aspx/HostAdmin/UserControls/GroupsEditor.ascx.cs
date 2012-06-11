#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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
using Subtext.Framework.Exceptions;
using Subtext.Web.Admin.WebUI.Controls;
using Subtext.Web.Properties;

namespace Subtext.Web.HostAdmin.UserControls
{
    /// <summary>
    ///	User control used to create, edit and delete Blog Groups.
    /// </summary>
    public partial class GroupsEditor : BaseUserControl
    {
        const string VSKEY_GROUPID = "VS_GROUPID";

        protected Button btnAddNewGroup = new Button();

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value></value>
        public int GroupId
        {
            get
            {
                if (ViewState[VSKEY_GROUPID] != null)
                {
                    return (int)ViewState[VSKEY_GROUPID];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState[VSKEY_GROUPID] = value; }
        }

        public bool IsActive { get; set; }

        bool CreatingGroup
        {
            get { return GroupId == NullValue.NullInt32; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddNewGroup.Click += btnAddNewGroup_Click;

            btnAddNewGroup.CssClass = "button";
            btnAddNewGroup.Text = Resources.Label_NewBlogGroup;
            ((HostAdminTemplate)Page.Master).AddSidebarControl(btnAddNewGroup);

            if (!IsPostBack)
            {
                chkShowInactive.Checked = false;
            }
            BindList();
        }

        private void BindList()
        {
            pnlResults.Visible = true;
            pnlEdit.Visible = false;

            ICollection<BlogGroup> groups = Repository.ListBlogGroups(!chkShowInactive.Checked);

            if (groups.Count > 0)
            {
                rprGroupsList.Visible = true;
                rprGroupsList.DataSource = groups;
                rprGroupsList.DataBind();
                lblNoMessages.Visible = false;
            }
            else
            {
                rprGroupsList.Visible = false;
                lblNoMessages.Visible = true;
            }
        }

        void BindEdit()
        {
            pnlResults.Visible = false;
            pnlEdit.Visible = true;

            BindEditHelp();

            if (!CreatingGroup)
            {
                BlogGroup group = Repository.GetBlogGroup(GroupId, false);
                if (group != null)
                {
                    txtTitle.Text = group.Title;
                    txtDescription.Text = group.Description;
                    txtDisplayOrder.Text = group.DisplayOrder.ToString(CultureInfo.InvariantCulture);
                    hfActive.Value = group.IsActive.ToString(CultureInfo.CurrentCulture);
                }
            }
        }

        // Contains the various help strings
        void BindEditHelp()
        {
            blogEditorHelp.HelpText = Resources.GroupsEditor_HelpTip;
        }

        protected void chkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            BindList();
        }

        private void btnAddNewGroup_Click(object sender, EventArgs e)
        {
            GroupId = NullValue.NullInt32;
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtDisplayOrder.Text = string.Empty;
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

        private void DeleteGroup()
        {
            if (Repository.DeleteBlogGroup(GroupId))
            {
                BindList();
            }
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
                messagePanel.ShowError(e.Message);
            }
            BindEdit();
        }

        // Saves a new blog group.  Any exceptions are propagated up to the caller.
        void SaveNewGroup()
        {
            int displayOrder;
            if (!Int32.TryParse(txtDisplayOrder.Text, out displayOrder))
            {
                displayOrder = NullValue.NullInt32;
            }

            var blogGroup = new BlogGroup
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                IsActive = true,
                DisplayOrder = displayOrder,
            };

            if (Repository.InsertBlogGroup(blogGroup) > 0)
            {
                messagePanel.ShowMessage(Resources.GroupsEditor_BlogGroupCreated);
            }
            else
            {
                messagePanel.ShowError(Resources.Message_UnexpectedError);
            }
        }

        // Saves changes to a blog group.  Any exceptions are propagated up to the caller.
        void SaveGroupEdits()
        {
            int displayOrder;
            if (!Int32.TryParse(txtDisplayOrder.Text, out displayOrder))
            {
                displayOrder = NullValue.NullInt32;
            }

            var blogGroup = new BlogGroup
            {
                Id = GroupId,
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                IsActive = Convert.ToBoolean(hfActive.Value),
                DisplayOrder = displayOrder,
            };


            if (Repository.UpdateBlogGroup(blogGroup))
            {
                messagePanel.ShowMessage(Resources.GroupsEditor_BlogGroupSaved);
            }
            else
            {
                messagePanel.ShowError(Resources.Message_UnexpectedError);
            }
        }

        protected static string ToggleActiveString(bool active)
        {
            if (active)
            {
                return Resources.Label_Deactivate;
            }
            else
            {
                return Resources.Label_Activate;
            }
        }

        void ToggleActive()
        {
            try
            {
                BlogGroup group = Repository.GetBlogGroup(GroupId, false);
                var blogGroup = new BlogGroup
                {
                    Id = GroupId,
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    IsActive = !IsActive,
                    DisplayOrder = group.DisplayOrder,
                };

                Repository.UpdateBlogGroup(blogGroup);
            }
            catch (BaseBlogConfigurationException e)
            {
                messagePanel.ShowError(e.Message);
            }

            BindList();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            messagePanel.ShowMessage(Resources.GroupsEditor_UpdateCancelled);
            BindList();
        }
    }
}