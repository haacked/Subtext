using System;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
    public partial class AdminPageTemplate : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void AddToActions(LinkButton lkb)
        {
            // HACK: one without the other doesn't seem to work. If I don't add this
            // to Items it doesn't render, if I don't add to controls it doesn't get
            // wired up. 
            LinksActions.Items.Add(lkb);
            LinksActions.Controls.Add(lkb);
        }

        public void AddToActions(HyperLink lnk)
        {
            LinksActions.Items.Add(lnk);
        }
        
        public BreadCrumbs BreadCrumb
        {
            get
            {
                return this.breadCrumbs;
            }
        }

        public string Title
        {
            get { return this.Page.Title; }
            set { this.Page.Title = value; }
        }
    }
}
