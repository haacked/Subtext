using System;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Articles
{
    public partial class Default : AdminPage
    {
        public Default()
        {
            TabSectionId = "Articles";
        }

        protected override void OnLoad(EventArgs e)
        {
            entries.HeaderText = Resources.Label_Articles;
            string message = Request.QueryString["message"];
            if(!string.IsNullOrEmpty(message))
            {
                Messages.ShowMessage(message);
            }
            base.OnLoad(e);
        }
    }
}