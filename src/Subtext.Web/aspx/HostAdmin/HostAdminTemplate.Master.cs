using System;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace Subtext.Web.HostAdmin
{
    /// <summary>
    /// Master page template for the host admin.
    /// </summary>
    public partial class HostAdminTemplate : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }

            if (hostAdminName != null && HostInfo.Instance != null)
            {
                hostAdminName.Text = HostInfo.Instance.HostUserName;
            }
        }

        /// <summary>
        /// Adds a control to the sidebar.
        /// </summary>
        /// <param name="control"></param>
        public void AddSidebarControl(Control control)
        {
            if (MPSidebar != null)
            {
                MPSidebar.Controls.Add(control);
            }
        }

        public BlogUrlHelper Url
        {
            get { return (Page as HostAdminPage).Url; }
        }
    }
}