using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.UserControls
{
	public partial class RoleChooser : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				this.rolesRepeater.DataSource = Roles.GetAllRoles();
				this.rolesRepeater.DataBind();
			}
		}

		public string ValidationGroup
		{
			get { return (string)this.ViewState["ValidationGroup"]; }
			set { this.ViewState["ValidationGroup"] = value; }
		}

		public StringCollection SelectedRoles
		{
			get
			{
				StringCollection roles = new StringCollection();
				foreach(RepeaterItem item in this.rolesRepeater.Items)
				{
					CheckBox checkBox = item.FindControl("roleCheckBox") as CheckBox;
					if(checkBox != null && checkBox.Checked)
					{
						roles.Add(checkBox.Text);
					}
				}
				return roles;
			}
		}
	}
}